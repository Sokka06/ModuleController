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
    }
    
    public class HandIKModule : AbstractIKModule
    {
        public float ArmLength = 1f;
        public float ShoulderWidth = 0.4f;
        public float ShoulderHeight = 1.5f;
        
        public List<HandIK> Hands;
        
        public override void UpdateModule(float deltaTime)
        {
            if (!Enabled)
                return;
            
            for (int i = 0; i < Hands.Count; i++)
            {
                var hasTarget = false;
                var targetPosition = Vector3.zero;
                var targetRotation = Quaternion.identity;
                var distance = 0f;
                
                var origin = Hands[i].Root.position;

                var hits = new RaycastHit[Controller.Character.LocalColliders.Count + 1];
                var count = Physics.RaycastNonAlloc(origin, Controller.Character.Transform.forward, hits, ArmLength);
                for (int j = 0; j < count; j++)
                {
                    var hit = hits[i];
                    if (Controller.Character.LocalColliders.Contains(hit.collider))
                        continue;
                    
                    hasTarget = true;
                    targetPosition = hit.point + hit.normal * 0.01f;
                    targetRotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(transform.forward, hit.normal), hit.normal);
                    distance = hit.distance;
                    break;
                }

                Hands[i].HasTarget = hasTarget;
                Hands[i].TargetPosition = targetPosition;
                Hands[i].TargetRotation = targetRotation;
                Hands[i].Distance = distance;
                
                if (hasTarget)
                {
                    var weight = 1f - Hands[i].Distance / ArmLength;
                    Controller.Listener.Animator.SetIKPositionWeight(Hands[i].IKGoal, weight);
                    //Controller.Listener.Animator.SetIKRotationWeight(Hands[i].IKGoal, weight);
                    
                    Controller.Listener.Animator.SetIKPosition(Hands[i].IKGoal, Hands[i].TargetPosition);
                    //Controller.Listener.Animator.SetIKRotation(Hands[i].IKGoal, Hands[i].TargetRotation);
                }
                else
                {
                    Controller.Listener.Animator.SetIKPositionWeight(Hands[i].IKGoal, 0f);
                    //Controller.Listener.Animator.SetIKRotationWeight(Hands[i].IKGoal, 0f);
                }
            }
        }

        private void OnDrawGizmos()
        {
            var color = Color.blue;
            color.a *= 0.25f;
            Gizmos.color = color;

            var origin = transform.position + transform.up * ShoulderHeight;
            var p0 = origin + transform.right * ShoulderWidth * 0.5f;
            var p1 = origin - transform.right * ShoulderWidth * 0.5f;
            Gizmos.DrawLine(p0, p1);
            
            Gizmos.DrawLine(p0, p0 + transform.forward * ArmLength);
            Gizmos.DrawLine(p1, p1 + transform.forward * ArmLength);
        }
    }
}