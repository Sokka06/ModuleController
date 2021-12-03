using System.Collections;
using System.Collections.Generic;
using Demos.Vehicle;
using UnityEngine;

namespace Demos.Common
{
    public class InputVehicleModule : AbstractInputModule
    {
        public VehicleController Vehicle;

        private VehicleInputModule _inputModule;

        public override void SetupModule(PlayerInputController controller)
        {
            base.SetupModule(controller);

            _inputModule = Vehicle.GetModule<VehicleInputModule>();
        }

        public override void UpdateModule(float deltaTime)
        {
            //Vehicle inputs
            var throttleInput = 0f;
            var steerInput = 0f;
            var brakeInput = 0f;
            
            if (Controller.Keyboard != null && (Controller.InputDevice == PlayerInputDevice.KeyboardAndMouse || Controller.InputDevice == PlayerInputDevice.Any))
            {
                //Up input
                if (Controller.Keyboard.wKey.isPressed || Controller.Keyboard.upArrowKey.isPressed)
                    throttleInput += 1f;
                //Down input
                if (Controller.Keyboard.sKey.isPressed || Controller.Keyboard.downArrowKey.isPressed)
                    brakeInput += 1f;
                //Left input
                if (Controller.Keyboard.aKey.isPressed || Controller.Keyboard.leftArrowKey.isPressed)
                    steerInput -= 1f;
                //Right input
                if (Controller.Keyboard.dKey.isPressed || Controller.Keyboard.rightArrowKey.isPressed)
                    steerInput += 1f;
            }
            
            if (Controller.Gamepad != null && (Controller.InputDevice == PlayerInputDevice.Gamepad || Controller.InputDevice == PlayerInputDevice.Any))
            {
                steerInput += Controller.Gamepad.leftStick.ReadValue().x;
                brakeInput += Controller.Gamepad.leftTrigger.ReadValue();
                throttleInput += Controller.Gamepad.rightTrigger.ReadValue();
            }
            
            //Collect inputs and send them to the character.
            var vehicleInputs = new VehicleInputs
            {
                Throttle = throttleInput,
                Brake = brakeInput,
                Steer = steerInput
            };
            _inputModule.SetInputs(ref vehicleInputs);
        }
    }
}