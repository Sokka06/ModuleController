using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Sokka06.ModuleController;
using UnityEngine;

namespace Demos.Demo1
{
    public class FirstPersonCameraController : ModuleControllerBehaviour<FirstPersonCameraController, AbstractCameraModule>
    {
        public CharacterModuleController CharacterController;
        public float HeadHeight = 0.1f;

        private Vector3 _prevPosition;
        private Transform _cameraTransform;
        
        public float Height { get; private set; }
        public Vector3 Offset { get; private set; } 
        
        protected override void Awake()
        {
            base.Awake();
            
            _cameraTransform = Camera.main.transform;
            
            Height = CharacterController.CharacterController.height - HeadHeight;
            Offset = Vector3.zero;
            _prevPosition = GetTargetPosition();
        }

        private void Start()
        {
            SetupModules();
        }

        private void Update()
        {
            var deltaTime = Time.deltaTime;
            
            UpdateModules(deltaTime);

            var position = InterpolateFixed(GetTargetPosition(), _prevPosition);
            _prevPosition = position;

            transform.position = position;
        }
        
        private Vector3 InterpolateFixed(Vector3 target, Vector3 previous)
        {
            // https://answers.unity.com/questions/1119684/interpolating-between-fixedupdate-frames.html
            var t = (Time.time - Time.fixedTime) / Time.fixedDeltaTime;
            return Vector3.Lerp(previous, target, t);
        }
        
        private Vector3 GetTargetPosition()
        {
            var rotation = Quaternion.LookRotation(_cameraTransform.forward, CharacterController.Transform.up);
            var offset = rotation * GetTotalOffset();
            return CharacterController.Transform.position + (CharacterController.Transform.up * Height) + offset;
        }

        private Vector3 GetTotalOffset()
        {
            var offset = Vector3.zero;

            for (int i = 0; i < Modules.Count; i++)
            {
                offset += Modules[i].Offset;
            }
            
            return offset;
        }
    }
}