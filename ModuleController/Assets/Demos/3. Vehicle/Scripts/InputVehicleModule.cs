using System.Collections;
using System.Collections.Generic;
using Demos.Common;
using Demos.Vehicle;
using UnityEngine;

namespace Demos.Vehicle
{
    /// <summary>
    /// Sends Player Inputs to Vehicle.
    /// </summary>
    public class InputVehicleModule : AbstractInputModule
    {
        public Vehicle Vehicle;

        private VehicleInputModule _inputModule;

        public override void SetupModule(PlayerInputController controller)
        {
            base.SetupModule(controller);

            AssignVehicle(Vehicle);
        }

        public void AssignVehicle(Vehicle vehicle)
        {
            Vehicle = vehicle;
            if (Vehicle == null)
            {
                _inputModule = null;
                return;
            }
            _inputModule = Vehicle.Controller.GetModule<VehicleInputModule>();
        }

        public override void UpdateModule(float deltaTime)
        {
            if (_inputModule == null)
                return;
            
            //Vehicle inputs
            var throttleInput = 0f;
            var steerInput = 0f;
            var brakeInput = 0f;
            var handbrakeInput = 0f;
            
            if (Controller.Keyboard != null && (Controller.InputDevice == PlayerInputDevice.KeyboardAndMouse || Controller.InputDevice == PlayerInputDevice.Any))
            {
                //Throttle input
                if (Controller.Keyboard.wKey.isPressed || Controller.Keyboard.upArrowKey.isPressed)
                    throttleInput += 1f;
                //Left input
                if (Controller.Keyboard.aKey.isPressed || Controller.Keyboard.leftArrowKey.isPressed)
                    steerInput -= 1f;
                //Right input
                if (Controller.Keyboard.dKey.isPressed || Controller.Keyboard.rightArrowKey.isPressed)
                    steerInput += 1f;
                //Brake input
                if (Controller.Keyboard.sKey.isPressed || Controller.Keyboard.downArrowKey.isPressed)
                    brakeInput += 1f;
                //Handbrake input
                if (Controller.Keyboard.spaceKey.isPressed)
                    handbrakeInput += 1f;
            }
            
            if (Controller.Gamepad != null && (Controller.InputDevice == PlayerInputDevice.Gamepad || Controller.InputDevice == PlayerInputDevice.Any))
            {
                steerInput += Controller.Gamepad.leftStick.ReadValue().x;
                brakeInput += Controller.Gamepad.leftTrigger.ReadValue();
                throttleInput += Controller.Gamepad.rightTrigger.ReadValue();
                handbrakeInput += Controller.Gamepad.bButton.ReadValue();
            }
            
            //Collect inputs and send them to the character.
            var vehicleInputs = new VehicleInputs
            {
                Throttle = throttleInput,
                Brake = brakeInput,
                Steer = steerInput,
                Handbrake = handbrakeInput
            };
            _inputModule.SetInputs(ref vehicleInputs);
        }
    }
}