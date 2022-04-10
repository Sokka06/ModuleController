using System.Collections;
using System.Collections.Generic;
using Sokka06.ModuleController;
using UnityEngine;

namespace Demos.Vehicle
{
    public class VehicleRollingResistanceModule : AbstractVehicleModule
    {
        public float RollingResistance = 0.1f;

        private VehicleInputModule _inputModule;

        public override void SetupModule(VehicleController controller)
        {
            base.SetupModule(controller);

            _inputModule = Controller.GetModule<VehicleInputModule>();
        }

        public override void UpdateModule(float deltaTime)
        {
            if (!Enabled || !Controller.GroundData.IsGrounded || _inputModule.Inputs.Throttle > 0f || !(RollingResistance > 0f))
                return;

            var localVelocity = Controller.LocalVelocity;
            var localTargetVelocity = new Vector3(localVelocity.x, localVelocity.y, localVelocity.z * (1f / (1f + (RollingResistance * deltaTime))));
            var localVelocityDiff = localTargetVelocity - localVelocity;
            
            Controller.Rigidbody.AddForce(Controller.Transform.TransformVector(localVelocityDiff) / deltaTime, ForceMode.Acceleration);
        }
    }
}
