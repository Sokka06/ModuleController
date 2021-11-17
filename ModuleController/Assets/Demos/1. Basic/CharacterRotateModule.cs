using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demos
{
    public enum CharacterRotateMode
    {
        Input,
        Velocity
    }
    
    public class CharacterRotateModule : AbstractCharacterModule
    {
        [Header("Rotate"), Tooltip("Input = Rotate character towards Input, Velocity = Rotate character towards velocity of the character.")]
        public CharacterRotateMode RotateMode;
        public float Sharpness = 25f;

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
            
            //Rotate towards movement. Can be moved to its own module.
            GetLookDir(RotateMode, out var lookDir);
            if (lookDir != Vector3.zero)
            {
                var rotation = Quaternion.LookRotation(Sharpness > 0f ? Vector3.Slerp(Controller.Transform.forward, lookDir, Sharpness * deltaTime).normalized : lookDir, Controller.Transform.up);
                Controller.SetRotation(rotation);
            }
        }

        private void GetLookDir(CharacterRotateMode mode, out Vector3 lookDir)
        {
            lookDir = Vector3.zero;
            switch (mode)
            {
                case CharacterRotateMode.Input:
                    lookDir = new Vector3(_inputModule.Inputs.Move.x, 0f, _inputModule.Inputs.Move.y);
                    break;
                case CharacterRotateMode.Velocity:
                    lookDir = Controller.Velocity;
                    break;
            }

            lookDir.y = 0f;
            lookDir.Normalize();
        }
    }
}