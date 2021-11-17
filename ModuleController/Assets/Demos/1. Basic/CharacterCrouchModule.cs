using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demos
{
    public struct CrouchDebugData
    {
        public Vector3 Point1;
        public Vector3 Point2;
        public float Radius;
        public int HitCount;
        public Collider[] Results;
    }
    
    /// <summary>
    /// Changes the height of the Character Controller to simulate crouching.
    /// </summary>
    public class CharacterCrouchModule : AbstractCharacterModule
    {
        [Header("Crouch")]
        public float CrouchHeight = 1f;

        private CharacterInputModule _inputModule;
        private float _initialHeight;
        
        public bool IsCrouching { get; private set; }

        public CrouchDebugData DebugData { get; private set; }

        public override void SetupModule(CharacterModuleController controller)
        {
            base.SetupModule(controller);

            _inputModule = Controller.GetModule<CharacterInputModule>();

            _initialHeight = Controller.CharacterController.height;
        }

        public override void UpdateModule(float deltaTime)
        {
            if (!Enabled)
                return;

            if (_inputModule.Inputs.Crouch)
            {
                if (!IsCrouching)
                {
                    Controller.SetHeight(CrouchHeight);
                    IsCrouching = true;
                }
            }
            else
            {
                if (IsCrouching && CanUncrouch())
                {
                    Controller.SetHeight(_initialHeight);
                    IsCrouching = false;
                }
            }
        }

        /// <summary>
        /// Uses OverlapCapsule to check for obstacles when uncrouhing.
        /// </summary>
        /// <returns>true if no obstacles found, false if an obstacle is found</returns>
        private bool CanUncrouch()
        {
            var origin = Controller.Transform.position;
            var up = Controller.Transform.up;
            var radius = Controller.CharacterController.radius;
            
            var point1 = origin + up * radius;
            var point2 = origin + up * (_initialHeight - radius);

            var results = new Collider[Controller.LocalColliders.Count + 1];
            var hitCount = Physics.OverlapCapsuleNonAlloc(point1, point2, radius, results);
            
            //Store debug data
            DebugData = new CrouchDebugData()
            {
                Point1 = point1,
                Point2 = point2,
                Radius = radius,
                HitCount = hitCount,
                Results = results
            };
            
            //If one of the hit colliders is not ours, can't uncrouch
            for (int i = 0; i < hitCount; i++)
            {
                if (!Controller.LocalColliders.Contains(results[i]))
                    return false;
            }

            //No obstacles, we can uncrouch
            return true;
        }

        private void OnDrawGizmosSelected()
        {
            if (Controller == null || DebugData.HitCount <= Controller.LocalColliders.Count)
                return;

            var color = Color.blue;
            color.a *= 0.25f;
            Gizmos.color = color;
            
            //draws obstacle check capsule.
            Gizmos.DrawSphere(DebugData.Point1, DebugData.Radius);
            Gizmos.DrawSphere(DebugData.Point2, DebugData.Radius);
        }
    }
}