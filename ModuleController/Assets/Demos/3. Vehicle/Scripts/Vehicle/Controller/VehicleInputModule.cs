using System.Collections;
using System.Collections.Generic;
using Demos.Vehicle;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Demos.Vehicle
{
    public struct VehicleInputs
    {
        //0f to 1f
        public float Throttle;
        // 0f to 1f
        public float Brake;
        // -1 to 1f
        public float Steer;
    }

    public class VehicleInputModule : AbstractVehicleModule
    {
        public VehicleInputs Inputs { get; private set; }

        public override void SetupModule(VehicleController controller)
        {
            base.SetupModule(controller);

            Inputs = new VehicleInputs();
        }

        public void SetInputs(ref VehicleInputs inputs)
        {
            Inputs = inputs;
        }

        public override void UpdateModule(float deltaTime)
        {
            if (!Enabled)
            {
                Inputs = new VehicleInputs();
                return;
            }
            
        }
    }
}