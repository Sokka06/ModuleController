using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demos
{
    public class CharacterJumpModule : AbstractCharacterModule
    {
        [Header("Jump")]
        public float Height = 1.5f;

        private CharacterInputModule _inputModule;
        private CharacterGravityModule _gravityModule;
        
        private bool _jumpRequested;

        public override void SetupModule(CharacterModuleController controller)
        {
            base.SetupModule(controller);

            _inputModule = Controller.GetModule<CharacterInputModule>();
            _gravityModule = Controller.GetModule<CharacterGravityModule>();
            
            //Some inputs can fall between fixed updates
            _inputModule.onInput += OnInput;
        }

        private void OnDestroy()
        {
            _inputModule.onInput -= OnInput;
        }

        private void OnInput()
        {
            if (_inputModule.Inputs.Jump)
            {
                _jumpRequested = true;
            }
        }

        public override void UpdateModule(float deltaTime)
        {
            if (!Enabled)
                return;

            if (_jumpRequested)
            {
                if (Controller.CharacterController.isGrounded)
                {
                    var jumpForce = Mathf.Sqrt(Height * -2.0f * _gravityModule.Gravity);
                    Controller.AddVelocity(Vector3.up * jumpForce - Vector3.Project(Controller.Velocity, Controller.CharacterController.transform.up));
                }
                
                _jumpRequested = false;
            }
        }
    }
}