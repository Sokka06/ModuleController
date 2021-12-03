using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demos.Demo2
{
    [Serializable]
    public class FeetIK
    {
        public AvatarIKGoal IKGoal;
        public Transform Root;
        public float PositionWeight = 1f;
        public float RotationWeight = 1f;

        public bool HasTarget { get; set; }
        public Vector3 TargetPosition { get; set; }
        public Quaternion TargetRotation { get; set; }
    }
    
    public class FeetIKModule : AbstractIKModule
    {
        [Header("Feet IK"), Range(0f,1f)]
        public float Weight = 1f;
        public LayerMask LayerMask = 3;

        public List<FeetIK> Feet;
        public float KneeHeight = 0.45f;
        public float Offset = 0.1f;

        public override void UpdateModule(float deltaTime)
        {
            if (!Enabled)
                return;

            for (int i = 0; i < Feet.Count; i++)
            {
                var hasTarget = false;
                var targetPosition = Vector3.zero;
                var targetRotation = Quaternion.identity;

                var origin = Feet[i].Root.position;//Controller.Updater.Animator.GetIKPosition(Feet[i].IKGoal);

                if (Physics.Raycast(origin + Controller.Character.Transform.up * KneeHeight, -Controller.Character.Transform.up, out var hit, KneeHeight * 2f, LayerMask))
                {
                    hasTarget = true;
                    targetPosition = hit.point + hit.normal * Offset;
                    targetRotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(transform.forward, hit.normal), hit.normal);
                }

                Feet[i].HasTarget = hasTarget;
                Feet[i].TargetPosition = targetPosition;
                Feet[i].TargetRotation = targetRotation;

                if (hasTarget)
                {
                    Controller.listener.Animator.SetIKPositionWeight(Feet[i].IKGoal, Feet[i].PositionWeight);
                    Controller.listener.Animator.SetIKRotationWeight(Feet[i].IKGoal, Controller.Character.CharacterController.velocity.sqrMagnitude > 1f ? 0f : Feet[i].RotationWeight);
                    
                    Controller.listener.Animator.SetIKPosition(Feet[i].IKGoal, Feet[i].TargetPosition);
                    Controller.listener.Animator.SetIKRotation(Feet[i].IKGoal, Feet[i].TargetRotation);
                }
                else
                {
                    Controller.listener.Animator.SetIKPositionWeight(Feet[i].IKGoal, 0f);
                    Controller.listener.Animator.SetIKRotationWeight(Feet[i].IKGoal, 0f);
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            for (int i = 0; i < Feet.Count; i++)
            {
                if (Feet[i].Root == null)
                    continue;
                
                //Gizmos.DrawSphere(Feet[i].Root.position + transform.up * KneeHeight, 0.01f);
            }
        }
    }
}