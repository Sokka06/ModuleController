using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demos.Demo2
{
    [Serializable]
    public class HandIK
    {
        public AvatarIKGoal IKGoal;
        public Transform Root;
        public float PositionWeight = 1f;
        public float RotationWeight = 1f;

        public bool HasTarget { get; set; }
        public Vector3 TargetPosition { get; set; }
        public Quaternion TargetRotation { get; set; }
        public float Weight { get; set; }
        public float Distance { get; set; }
        public float TouchTime { get; set; }
    }
    
    public class HandIKModule : AbstractIKModule
    {
        public List<HandIK> Hands;
        public float TouchDistance = 0.5f;
        public float TouchOffset = -0.15f;
        
        private float _wallTime;
        
        public override void UpdateModule(float deltaTime)
        {
            if (!Enabled)
                return;
            
            for (int i = 0; i < Hands.Count; i++)
            {
                var hand = Hands[i];
                
                var hasTarget = false;
                var targetPosition = Vector3.zero;
                var targetRotation = Quaternion.identity;
                var distance = 0f;
                var time = 0f;
                
                var origin = hand.Root.position + Controller.Character.Transform.up * TouchOffset;
                var direction = Controller.Character.Transform.forward;
                Debug.DrawLine(origin, origin + direction * TouchDistance);

                var hits = new RaycastHit[Controller.Character.LocalColliders.Count + 1];
                var count = Physics.RaycastNonAlloc(origin, direction, hits, TouchDistance);
                for (int j = 0; j < count; j++)
                {
                    var hit = hits[j];
                    if (Controller.Character.LocalColliders.Contains(hit.collider))
                        continue;

                    var dot = Vector3.Dot(direction, hit.normal);
                    if (Mathf.Abs(dot) < 0.5f)
                        continue;
                    
                    // Use first non local hit.
                    hasTarget = true;
                    targetPosition = hit.point + hit.normal * 0.01f;
                    targetRotation = Quaternion.LookRotation(Vector3.Cross(Controller.Character.Transform.right, hit.normal), hit.normal);
                    distance = hit.distance;
                    time = hand.TouchTime + deltaTime;
                    break;
                }

                hand.HasTarget = hasTarget;
                hand.TargetPosition = targetPosition;
                hand.TargetRotation = targetRotation;
                hand.Distance = distance;
                hand.TouchTime = time;
                
                if (hasTarget)
                {
                    hand.Weight = Mathf.Lerp(0f, 1f, Mathf.Clamp01(hand.TouchTime) / 0.5f);//1f - hand.Distance / ArmLength;
                    
                    Controller.Listener.Animator.SetIKPositionWeight(hand.IKGoal, hand.Weight);
                    Controller.Listener.Animator.SetIKRotationWeight(hand.IKGoal, hand.Weight);
                    
                    Controller.Listener.Animator.SetIKPosition(hand.IKGoal, hand.TargetPosition);
                    Controller.Listener.Animator.SetIKRotation(hand.IKGoal, hand.TargetRotation);
                }
                else
                {
                    hand.Weight = Mathf.Lerp(hand.Weight, 0f, deltaTime);
                    
                    Controller.Listener.Animator.SetIKPositionWeight(hand.IKGoal, hand.Weight);
                    Controller.Listener.Animator.SetIKRotationWeight(hand.IKGoal, hand.Weight);
                }
            }
        }

        private void OnDrawGizmos()
        {
            /*var color = Color.blue;
            color.a *= 0.25f;
            Gizmos.color = color;

            var origin = transform.position + transform.up * ShoulderHeight;
            var p0 = origin + transform.right * ShoulderWidth * 0.5f;
            var p1 = origin - transform.right * ShoulderWidth * 0.5f;
            Gizmos.DrawLine(p0, p1);
            
            Gizmos.DrawLine(p0, p0 + transform.forward * ArmLength);
            Gizmos.DrawLine(p1, p1 + transform.forward * ArmLength);*/

            for (int i = 0; i < Hands.Count; i++)
            {
                var hand = Hands[i];
                
                if (!hand.HasTarget)
                    return;
                
                Gizmos.DrawSphere(hand.TargetPosition, 0.05f);
                Gizmos.DrawRay(hand.TargetPosition, hand.TargetRotation * Vector3.up);
                Gizmos.DrawRay(hand.TargetPosition, hand.TargetRotation * Vector3.right);
                Gizmos.DrawRay(hand.TargetPosition, hand.TargetRotation * Vector3.forward);
            }
        }
    }
}