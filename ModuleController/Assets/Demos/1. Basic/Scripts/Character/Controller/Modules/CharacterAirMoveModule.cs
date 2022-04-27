using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demos
{
    
    /// <summary>
    /// Moves character when not on ground.
    /// </summary>
    public class CharacterAirMoveModule : AbstractCharacterModule
    {
        [Header("Air Move")]
        public float Speed = 5f;
        public float Acceleration = 1f;

        private CharacterInputModule _inputModule;
        private CharacterGravityModule _gravityModule;

        public override void SetupModule(CharacterModuleController controller)
        {
            base.SetupModule(controller);

            _inputModule = Controller.GetModule<CharacterInputModule>();
            _gravityModule = Controller.GetModule<CharacterGravityModule>();
        }

        public override void UpdateModule(float deltaTime)
        {
            if (!Enabled)
                return;
            
            var moveDir = new Vector3(_inputModule.Inputs.Move.x, 0f, _inputModule.Inputs.Move.y).normalized;
            
            if (!Controller.CharacterController.isGrounded && moveDir.sqrMagnitude > 0f)
            {
                var targetVelocity = moveDir * Speed * _inputModule.Inputs.Move.magnitude;

                var velocityDiff = Vector3.ProjectOnPlane(targetVelocity - Controller.Velocity, _gravityModule.Up);
                Controller.AddVelocity(velocityDiff * Acceleration * deltaTime);
            }
        }
    }
}