using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demos.Vehicle
{
    public class VehicleHandbrakeModule : AbstractVehicleModule
    {
        public List<CustomWheel> HandbrakedWheels;
        
        private VehicleInputModule _inputModule;
        private float _amount;

        public override void SetupModule(VehicleController controller)
        {
            base.SetupModule(controller);

            _inputModule = Controller.GetModule<VehicleInputModule>();
        }

        public override void UpdateModule(float deltaTime)
        {
            //if(!Controller.GroundData.IsGrounded)
            //    return;

            if (_inputModule.Inputs.Handbrake > 0f)
                _amount = _inputModule.Inputs.Handbrake;

            for (int i = 0; i < HandbrakedWheels.Count; i++)
            {
                HandbrakedWheels[i].Stiffness = Mathf.Lerp(1f, 0.2f, _amount);
            }

            if (_amount > 0f)
            {
                _amount = Mathf.Max(_amount - deltaTime, 0f);
            }
        }
    }
}
