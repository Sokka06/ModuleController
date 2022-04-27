using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demos.Demo2
{
    /// <summary>
    /// Adjusts pelvis height based on feet target height to make sure both feet will reach the ground.
    /// </summary>
    public class PelvisIKModule : AbstractIKModule
    {
        public float HeightOffset = -0.1f;
        public float MaxDrop = 0.5f;
        public float Smoothness = 25f;

        private FeetIKModule _feetIKModule;
        private float _height;

        public override void SetupModule(IKModuleController controller)
        {
            base.SetupModule(controller);

            _feetIKModule = Controller.GetModule<FeetIKModule>();

            _height = Controller.Character.Transform.position.y + (Controller.Character.CharacterController.height * 0.5f) + HeightOffset;
        }

        public override void UpdateModule(float deltaTime)
        {
            var offset = HeightOffset;
            
            for (int i = 0; i < _feetIKModule.Feet.Count; i++)
            {
                if (!Controller.Character.GroundData.HasGround || Controller.Character.Velocity.sqrMagnitude > 1f)
                    break;
                
                if (!_feetIKModule.Feet[i].HasTarget)
                {
                    offset = -MaxDrop;
                    continue;
                }
                
                var position = _feetIKModule.Feet[i].TargetPosition;
                var footOffset = position.y - Controller.Listener.Animator.transform.position.y;

                if (footOffset < offset)
                    offset = footOffset;
            }

            //Get hips position
            var newPosition = Controller.Listener.Animator.bodyPosition;
            var targetHeight = newPosition.y + offset;
            _height = Mathf.Lerp(_height, targetHeight, Smoothness * deltaTime);
            newPosition.y = _height;
            Controller.Listener.Animator.bodyPosition = newPosition;
        }
    }
}