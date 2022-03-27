using System;
using System.Collections;
using System.Collections.Generic;
using Demos.Common;
using UnityEngine;

namespace Demos.Vehicle
{
    public class PlayerDriver : AbstractDriver
    {
        [Header("Player")]
        public PlayerInputController InputController;

        private InputVehicleModule _inputModule;

        private void OnValidate()
        {
            if (InputController == null)
                InputController = GetComponentInChildren<PlayerInputController>();
        }

        public override void Setup(Vehicle vehicle)
        {
            _inputModule = InputController.GetModule<InputVehicleModule>();
            
            base.Setup(vehicle);
        }

        public override void AssignVehicle(Vehicle vehicle)
        {
            base.AssignVehicle(vehicle);
            
            _inputModule.AssignVehicle(vehicle);
        }
    }
}