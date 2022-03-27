using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demos.Demo1
{
    public class CameraCrouchModule : AbstractCameraModule
    {
        [Header("Crouch")]
        public float CrouchHeight = 0.9f;
        public float CrouchDuration = 0.2f;
        
        private CharacterCrouchModule _crouchModule;
        
        private bool _prevIsCrouching;
        private float _initialHeight;

        private float _startHeight;
        private float _targetHeight;
        private float _crouchTimer;

        public override void SetupModule(FirstPersonCameraController controller)
        {
            base.SetupModule(controller);
            
            _crouchModule = Controller.CharacterController.GetModule<CharacterCrouchModule>();

            _initialHeight = Controller.Height;
            SetCrouch(_initialHeight, _initialHeight);
        }

        public override void UpdateModule(float deltaTime)
        {
            if (!Enabled)
                return;

            if (_crouchModule.IsCrouching && !_prevIsCrouching)
            {
                //Started crouching
                SetCrouch(Controller.Height, CrouchHeight);
            }
            else if (!_crouchModule.IsCrouching && _prevIsCrouching)
            {
                //Stopped crouching
                SetCrouch(Controller.Height, _initialHeight);
            }
            
            UpdateCrouch(deltaTime);
            
            _prevIsCrouching = _crouchModule.IsCrouching;
        }
        
        private void UpdateCrouch(float deltaTime)
        {
            var duration = Mathf.Max(CrouchDuration, 0.001f);
            _crouchTimer += deltaTime;
            if (_crouchTimer > duration) {
                _crouchTimer = duration;
            }
            
            var t = _crouchTimer / duration;
            Controller.Height = Mathf.Lerp(_startHeight, _targetHeight, t);
        }

        private void SetCrouch(float start, float target)
        {
            _crouchTimer = 0f;
            _startHeight = start;
            _targetHeight = target;
        }
    }
}