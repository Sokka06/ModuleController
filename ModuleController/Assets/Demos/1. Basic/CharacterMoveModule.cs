using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demos
{
    public enum CharacterMoveMode
    {
        Input,
        Forward
    }
    
    public class CharacterMoveModule : AbstractCharacterModule
    {
        [Header("Move"), Tooltip("Input = Moves towards input, Forward = Moves towards current forward direction.")] 
        public CharacterMoveMode MoveMode;
        
        [Space]
        public float SpeedGrounded = 5f;
        public float Smoothness = 30f;
        
        [Space]
        public float SpeedAir = 5f;
        public float Acceleration = 30f;

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

            GetMoveDir(MoveMode, out var moveDir);
            
            var targetMovementVelocity = Vector3.zero;
            if (Controller.CharacterController.isGrounded)
            {
                // Calculate target velocity
                Vector3 inputRight = Vector3.Cross(moveDir, Controller.CharacterController.transform.up);
                targetMovementVelocity = moveDir * SpeedGrounded *  _inputModule.Inputs.Move.magnitude;

                // Smooth movement Velocity
                Controller.SetVelocity(Vector3.Lerp(Controller.Velocity, targetMovementVelocity, Smoothness * deltaTime));
            }
            else
            {
                // Add move input
                if (moveDir.sqrMagnitude > 0f)
                {
                    targetMovementVelocity = moveDir * SpeedAir * _inputModule.Inputs.Move.magnitude;

                    var velocityDiff = Vector3.ProjectOnPlane(targetMovementVelocity - Controller.Velocity, Vector3.up);
                    Controller.AddVelocity(velocityDiff * Acceleration * deltaTime);
                }
            }
        }
        
        private void GetMoveDir(CharacterMoveMode mode, out Vector3 moveDir)
        {
            moveDir = Vector3.zero;
            switch (mode)
            {
                case CharacterMoveMode.Input:
                    moveDir = new Vector3(_inputModule.Inputs.Move.x, 0f, _inputModule.Inputs.Move.y);
                    break;
                case CharacterMoveMode.Forward:
                    moveDir = Controller.Transform.forward;
                    break;
            }

            moveDir.y = 0f;
            moveDir.Normalize();
        }
    }
}