using System.Collections;
using System.Collections.Generic;
using Sokka06.ModuleController;
using UnityEngine;

namespace Demos.Demo1
{
    public class FirstPersonCameraController : ModuleControllerBehaviour<FirstPersonCameraController, AbstractCameraModule>
    {
        public CharacterModuleController CharacterController;

        private Vector3 _prevPosition;
        
        public float Height { get; set; }
        public Vector3 Offset { get; set; } 
        
        private void Awake()
        {
            Height = CharacterController.CharacterController.height;
            Offset = Vector3.zero;
            _prevPosition = GetTargetPosition();

            SetupModules();
        }

        private void LateUpdate()
        {
            var deltaTime = Time.deltaTime;
            
            UpdateModules(deltaTime);
            
            UpdatePosition();
        }

        private void UpdatePosition()
        {
            // https://answers.unity.com/questions/1119684/interpolating-between-fixedupdate-frames.html
            var t = (Time.time - Time.fixedTime) / Time.fixedDeltaTime;
            var newPosition = Vector3.Lerp(_prevPosition, GetTargetPosition(), t);

            transform.position = newPosition;

            _prevPosition = newPosition;
        }
        
        private Vector3 GetTargetPosition()
        {
            var height = Height - 0.1f;
            var offset = CharacterController.Transform.rotation * Offset;
            return CharacterController.Transform.position + CharacterController.Transform.up * height + offset;
        }
    }
}