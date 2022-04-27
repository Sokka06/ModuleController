using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demos.Demo2
{
    public struct LookIKDebugData
    {
        public Ray Ray;
        public Vector3 Point;
    }
    
    public class LookIKModule : AbstractIKModule
    {
        [Header("Look IK"), Range(0f,1f)]
        public float Weight;

        public LayerMask LayerMask = 3;
        public float Sharpness = 10f;

        [Space]
        public float Distance = 10F;
        
        private Transform _cameraTransform;
        private Vector3 _lookAtPosition;
        
        public LookIKDebugData DebugData { get; private set; }

        public override void SetupModule(IKModuleController controller)
        {
            base.SetupModule(controller);

            _cameraTransform = Camera.main.transform;
            _lookAtPosition = GetLookAtPosition();
        }

        public override void UpdateModule(float deltaTime)
        {
            if (!Enabled)
                return;
            
            _lookAtPosition = Vector3.Lerp(_lookAtPosition, GetLookAtPosition(), Sharpness * deltaTime);
            
            Controller.Listener.Animator.SetLookAtWeight(GetLookAtWeight());
            Controller.Listener.Animator.SetLookAtPosition(_lookAtPosition);
        }

        private Vector3 GetLookAtPosition()
        {
            var ray = new Ray(_cameraTransform.position, _cameraTransform.forward);
            /* disabled for now.
            var results = new RaycastHit[Controller.Character.LocalColliders.Count + 1];
            var count = Physics.RaycastNonAlloc(ray, results, Distance, LayerMask);

            var lookAtPosition = ray.origin + ray.direction * Distance;
            for (int i = 0; i < count; i++)
            {
                if (Controller.Character.LocalColliders.Contains(results[i].collider))
                    continue;

                lookAtPosition = results[i].point;
                break;
            }
            */
            
            var lookAtPosition = _cameraTransform.position + _cameraTransform.forward * Distance;

            DebugData = new LookIKDebugData()
            {
                Ray = ray,
                Point = lookAtPosition,
            };
            
            return lookAtPosition;
        }

        private float GetLookAtWeight()
        {
            //Fade out weight if trying to look behind the character.
            
            var lookDir = (_lookAtPosition - Controller.Character.Transform.position).normalized;
            var dot = Vector3.Dot(Controller.Character.Transform.forward,lookDir);
            
            return dot < 0f ? 1f - Mathf.Abs(dot) : Weight;
        }

        private void OnDrawGizmosSelected()
        {
            if (Controller == null)
                return;
            
            Gizmos.DrawLine(DebugData.Ray.origin, DebugData.Ray.origin + DebugData.Ray.direction * Distance);
            Gizmos.DrawSphere(DebugData.Point, 0.05f);
        }
    }
}