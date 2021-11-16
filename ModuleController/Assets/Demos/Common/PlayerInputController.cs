using System;
using System.Collections;
using System.Collections.Generic;
using Demos;
using UnityEngine;
using UnityEngine.InputSystem;


namespace Demos
{
    public class PlayerInputController : MonoBehaviour
    {
        public CharacterModuleController Controller;

        private CharacterInputModule _inputModule;
        private Transform _cameraTransform;
    
        private void Awake()
        {
            _cameraTransform = Camera.main.transform;
        
            _inputModule = Controller.GetModule<CharacterInputModule>();
            
            InputSystem.onAfterUpdate += OnAfterUpdate;
        }

        private void OnDestroy()
        {
            InputSystem.onAfterUpdate -= OnAfterUpdate;
        }

        private void OnAfterUpdate()
        {
            var moveInput = new Vector2();
            var jumpDown = false;
            var sprintInput = false;
            var crouchInput = false;
            
            var keyboard = Keyboard.current;
            if (keyboard != null)
            {
                //Up input
                if (keyboard.wKey.isPressed || keyboard.upArrowKey.isPressed)
                    moveInput.y += 1f;
                //Down input
                if (keyboard.sKey.isPressed || keyboard.downArrowKey.isPressed)
                    moveInput.y -= 1f;
                //Left input
                if (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed)
                    moveInput.x -= 1f;
                //Right input
                if (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed)
                    moveInput.x += 1f;

                jumpDown = keyboard.spaceKey.wasPressedThisFrame;
                sprintInput = keyboard.leftShiftKey.isPressed;
                crouchInput = keyboard.cKey.isPressed;
            }
            
            var gamepad = Gamepad.current;
            if (gamepad != null)
            {
                moveInput += gamepad.leftStick.ReadValue();

                jumpDown |= gamepad.buttonNorth.wasPressedThisFrame;
                sprintInput |= gamepad.buttonWest.isPressed;
                crouchInput |= gamepad.buttonEast.isPressed;
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