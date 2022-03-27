using System;
using System.Collections;
using System.Collections.Generic;
using Sokka06.ModuleController;
using UnityEngine;

namespace Demos.Demo2
{
    public class CharacterViewController : ModuleControllerBehaviour<CharacterViewController, AbstractViewModule>
    {
        public CharacterModuleController Character;
        public Transform Root;
        
        private Vector3 _prevPosition;

        protected override void Awake()
        {
            base.Awake();
            _prevPosition = GetTargetPosition();
        }

        private void Start()
        {
            SetupModules();
        }

        private void LateUpdate()
        {
            UpdateModules(Time.deltaTime);

            UpdatePosition();
        }
        
        private void UpdatePosition()
        {
            // https://answers.unity.com/questions/1119684/interpolating-between-fixedupdate-frames.html
            var t = (Time.time - Time.fixedTime) / Time.fixedDeltaTime;
            var newPosition = Vector3.Lerp(_prevPosition, GetTargetPosition(), t);

            Root.position = newPosition;

            _prevPosition = newPosition;
        }
        
        private Vector3 GetTargetPosition()
        {
            return Character.Transform.position;
        }
    }
}