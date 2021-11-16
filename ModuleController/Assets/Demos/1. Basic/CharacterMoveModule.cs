using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demos
{
    public class CharacterMoveModule : AbstractCharacterModule
    {
        [Header("Move")]
        public float SpeedGrounded = 5f;
        public float Smoothness = 30f;
        
        [Header("Air")]
        public float SpeedAir = 5f;
        public float AccelerationAir = 30f;

        private CharacterInputModule _inputModule;

        public override void SetupModule(CharacterModuleController controller)
        {
            base.SetupModule(controller);

            _inputModule = Controller.GetModule<CharacterInputModule>();
        }

        public override void UpdateModule(float deltaTime)
        {
            if (!Enabled)
                return;

            var dir = new Vector3(_inputModule.Inputs.Move.x, 0f, _inputModule.Inputs.Move.y);
            
            var targetMovementVelocity = Vector3.zero;
            if (Controller.CharacterController.isGrounded)
            {
                // Calculate target velocity
                Vector3 inputRight = Vector3.Cross(dir, Controller.CharacterController.transform.up);
                targetMovementVelocity = dir * SpeedGrounded;

                // Smooth movement Velocity
                Controller.SetVelocity(Vector3.Lerp(Controller.Velocity, targetMovementVelocity, Smoothness * deltaTime));
            }
            else
            {
                // Add move input
                if (dir.sqrMagnitude > 0f)
                {
                    targetMovementVelocity = dir * SpeedAir;

                    var velocityDiff = Vector3.ProjectOnPlane(targetMovementVelocity - Controller.Velocity, Vector3.up);
                    Controller.AddVelocity(velocityDiff * AccelerationAir * deltaTime);
                }
            }
        }
    }
}