using System.Collections;
using System.Collections.Generic;
using Demos.Demo1;
using UnityEngine;

namespace Demos.Demo1
{
    /// <summary>
    /// Simulates crouching by scaling down the root transform.
    /// </summary>
    public class ViewCrouchModule : AbstractViewModule
    {
        [Header("Crouch")]
        public Transform Root;
        public float CrouchDuration = 0.2f;
        
        private CharacterCrouchModule _crouchModule;
        
        private bool _prevIsCrouching;
        private Vector3 _initialScale;

        private Vector3 _startScale;
        private Vector3 _targetScale;
        private float _crouchTimer;

        public override void SetupModule(CapsuleViewController controller)
        {
            base.SetupModule(controller);

            _crouchModule = Controller.Character.GetModule<CharacterCrouchModule>();

            _initialScale = Root.localScale;
            SetCrouch(_initialScale, _initialScale);
        }

        public override void UpdateModule(float deltaTime)
        {
            if (!Enabled)
                return;

            if (_crouchModule.IsCrouching && !_prevIsCrouching)
            {
                //Started crouching
                var targetScale = _initialScale;
                targetScale.y *= 0.5f;
                SetCrouch(Root.localScale, targetScale);
            }
            else if (!_crouchModule.IsCrouching && _prevIsCrouching)
            {
                //Stopped crouching
                SetCrouch(Root.localScale, _initialScale);
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
            Root.localScale = Vector3.Lerp(_startScale, _targetScale, t);
        }

        private void SetCrouch(Vector3 start, Vector3 target)
        {
            _crouchTimer = 0f;
            _startScale = start;
            _targetScale = target;
        }
    }
}