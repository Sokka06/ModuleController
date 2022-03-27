using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Demos.Vehicle;
using UnityEngine;

namespace Demos.Common
{
    public class InputCameraModule : AbstractInputModule
    {
        public CinemachineVirtualCameraBase VirtualCamera;

        private PlayerInputProvider _inputProvider;
    
        public override void SetupModule(PlayerInputController controller)
        {
            base.SetupModule(controller);
        
            AssignCamera(VirtualCamera);
        }

        public void AssignCamera(CinemachineVirtualCameraBase virtualCamera)
        {
            VirtualCamera = virtualCamera;
            if (VirtualCamera == null)
            {
                _inputProvider = null;
                return;
            }
            
            _inputProvider = VirtualCamera.GetComponent<PlayerInputProvider>();
        }

        public override void UpdateModule(float deltaTime)
        {
            if(_inputProvider == null)
                return;
            
            //Camera inputs
            var axis = new Vector2();
            
            if (Controller.Mouse != null && (Controller.InputDevice == PlayerInputDevice.KeyboardAndMouse || Controller.InputDevice == PlayerInputDevice.Any))
            {
                axis = Controller.Mouse.delta.ReadValue();
            }
            
            if (Controller.Gamepad != null && (Controller.InputDevice == PlayerInputDevice.Gamepad || Controller.InputDevice == PlayerInputDevice.Any))
            {
                axis += Controller.Gamepad.rightStick.ReadValue();
            }
            
            var cameraInputs = new PlayerCameraInputs()
            {
                Axis = axis
            };
        
            _inputProvider.SetInputs(ref cameraInputs);
        }
    }
}