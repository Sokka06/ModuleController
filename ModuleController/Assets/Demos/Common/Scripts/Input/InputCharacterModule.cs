using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Demos.Vehicle;
using UnityEngine;

namespace Demos.Common
{
    public class InputCharacterModule : AbstractInputModule
    {
        public CharacterModuleController Character;

        private CharacterInputModule _inputModule;
        private Transform _cameraTransform;
    
        public override void SetupModule(PlayerInputController controller)
        {
            base.SetupModule(controller);
            
            _cameraTransform = Camera.main.transform;

            _inputModule = Character.GetModule<CharacterInputModule>();
        }

        public override void UpdateModule(float deltaTime)
        {
            //Character inputs
            var moveInput = new Vector2();
            var jumpDown = false;
            var sprintInput = false;
            var crouchInput = false;

            if (Controller.Keyboard != null && (Controller.InputDevice == PlayerInputDevice.KeyboardAndMouse || Controller.InputDevice == PlayerInputDevice.Any))
            {
                //Up input
                if (Controller.Keyboard.wKey.isPressed || Controller.Keyboard.upArrowKey.isPressed)
                    moveInput.y += 1f;
                //Down input
                if (Controller.Keyboard.sKey.isPressed || Controller.Keyboard.downArrowKey.isPressed)
                    moveInput.y -= 1f;
                //Left input
                if (Controller.Keyboard.aKey.isPressed || Controller.Keyboard.leftArrowKey.isPressed)
                    moveInput.x -= 1f;
                //Right input
                if (Controller.Keyboard.dKey.isPressed || Controller.Keyboard.rightArrowKey.isPressed)
                    moveInput.x += 1f;

                jumpDown = Controller.Keyboard.spaceKey.wasPressedThisFrame;
                sprintInput = Controller.Keyboard.leftShiftKey.isPressed;
                crouchInput = Controller.Keyboard.cKey.isPressed;
            }
            
            if (Controller.Gamepad != null && (Controller.InputDevice == PlayerInputDevice.Gamepad || Controller.InputDevice == PlayerInputDevice.Any))
            {
                moveInput += Controller.Gamepad.leftStick.ReadValue();

                jumpDown |= Controller.Gamepad.buttonNorth.wasPressedThisFrame;
                sprintInput |= Controller.Gamepad.buttonWest.isPressed;
                crouchInput |= Controller.Gamepad.buttonEast.isPressed;
            }
            
            //Camera relative movement
            RelativeToTransform(ref moveInput, _cameraTransform);

            //Collect inputs and send them to the character.
            var characterInputs = new CharacterInputs
            {
                Move = moveInput,
                Jump = jumpDown,
                Sprint = sprintInput,
                Crouch = crouchInput
            };
            _inputModule.SetInputs(ref characterInputs);
        }
        
        private void RelativeToTransform(ref Vector2 vector, Transform relativeTransform)
        {
            var cameraPlanarDirection = Vector3.ProjectOnPlane(relativeTransform.rotation * Vector3.forward, Vector3.up).normalized;
            if (cameraPlanarDirection.sqrMagnitude == 0f)
            {
                cameraPlanarDirection = Vector3.ProjectOnPlane(relativeTransform.rotation * Vector3.up, Vector3.up).normalized;
            }
            var cameraPlanarRotation = Quaternion.LookRotation(cameraPlanarDirection, Vector3.up);

            var newVector = cameraPlanarRotation * new Vector3(vector.x, 0f, vector.y);
        
            vector = new Vector2(newVector.x, newVector.z);
        }
    }
}