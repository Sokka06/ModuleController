using System;
using System.Collections;
using System.Collections.Generic;
using Demos.Vehicle;
using UnityEngine;

namespace Demos.Vehicle
{
    public class VehicleDriveModule : AbstractVehicleModule
    {
        [Header("Drive Module")]
        public List<WheelCollider> PoweredWheels;
        [Space]
        public float Speed = 20f;
        public float Acceleration = 2f;

        private VehicleInputModule _inputModule;
        private VehicleGearboxModule _gearboxModule;
        
        public bool IsReversing { get; private set; }

        public override void SetupModule(VehicleController controller)
        {
            base.SetupModule(controller);

            _inputModule = Controller.GetModule<VehicleInputModule>();
            _gearboxModule = Controller.GetModule<VehicleGearboxModule>();
        }

        public override void UpdateModule(float deltaTime)
        {
            IsReversing = _gearboxModule.Gearbox.GetRatio() < 0f;
            var input = !IsReversing
                ? _inputModule.Inputs.Throttle
                : _inputModule.Inputs.Brake;
            
            if (!Enabled || !Controller.GroundData.IsGrounded || !(input > 0f))
                return;

            var targetSpeed = Speed * _gearboxModule.Gearbox.GetRatio() * input;

            for (int i = 0; i < PoweredWheels.Count; i++)
            {
                var isGrounded = PoweredWheels[i].GetGroundHit(out var wheelHit);
                
                if (!isGrounded)
                    continue;

                var forward = Vector3.Cross(wheelHit.sidewaysDir, wheelHit.normal);
                var velocity = Controller.PointVelocity(wheelHit.point);
                var velocityDiff = (forward * targetSpeed) - velocity;
                var force = velocityDiff * Acceleration;
                
                Controller.AddVelocity(force / PoweredWheels.Count, wheelHit.point, ForceMode.Acceleration);
            }
        }

        /*private void OnGUI()
        {
            GUI.Label(new Rect(0f, 0f, Screen.width, 32), $"Local Velocity {Controller.Transform.InverseTransformVector(Controller.Rigidbody.velocity)}");
        }*/
    }
}