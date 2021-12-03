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
        public float Speed = 20f;
        public float Acceleration = 2f;

        private VehicleInputModule _inputModule;
        private VehicleGearboxModule _gearboxModule;

        public override void SetupModule(VehicleController controller)
        {
            base.SetupModule(controller);

            _inputModule = Controller.GetModule<VehicleInputModule>();
            _gearboxModule = Controller.GetModule<VehicleGearboxModule>();
        }

        public override void UpdateModule(float deltaTime)
        {
            var input = _gearboxModule.Gearbox.GetRatio() >= 0f
                ? _inputModule.Inputs.Throttle
                : _inputModule.Inputs.Brake;
            
            if (!Enabled || !Controller.GroundData.IsGrounded || !(input > 0f))
                return;

            //var velocity = Controller.Rigidbody.velocity;

            //var direction = Vector3.Cross(Controller.Transform.right, Controller.GroundData.Normal);
            //var targetVelocity = direction * Speed * _inputModule.Inputs.Throttle;
            
            var targetSpeed = Speed * _gearboxModule.Gearbox.GetRatio() * input;

            /*var localVelocity = Controller.Transform.InverseTransformVector(Controller.Rigidbody.velocity);
            
            var targetLocalVelocity = localVelocity;
            targetLocalVelocity.z = Speed * _inputModule.Inputs.Throttle;
            targetLocalVelocity.z -= Speed * _inputModule.Inputs.Brake;

            var velocityDiff = targetLocalVelocity - localVelocity;*/

            //var velocityDiff = Vector3.ProjectOnPlane(targetVelocity - velocity, Vector3.up);

            //var force = velocityDiff * Acceleration;
            for (int i = 0; i < PoweredWheels.Count; i++)
            {
                var isGrounded = PoweredWheels[i].GetGroundHit(out var wheelHit);
                if (!isGrounded)
                    continue;

                var forward = Vector3.Cross(wheelHit.sidewaysDir, wheelHit.normal);
                var velocity = Controller.Rigidbody.GetPointVelocity(wheelHit.point);
                var velocityDiff = Vector3.ProjectOnPlane((forward * targetSpeed) - velocity, Vector3.up);
                var force = velocityDiff * Acceleration;
                
                Controller.Rigidbody.AddForceAtPosition(force / PoweredWheels.Count, wheelHit.point, ForceMode.Acceleration);
            }
            //Controller.Rigidbody.AddForce(velocityDiff * Acceleration, ForceMode.Acceleration);
            
            for (int i = 0; i < Controller.Wheels.Count; i++)
            {
                //Controller.Wheels[i].motorTorque = _inputModule.Inputs.Throttle * 1000f;
            }
        }

        private void OnGUI()
        {
            GUI.Label(new Rect(0f, 0f, Screen.width, 32), $"Local Velocity {Controller.Transform.InverseTransformVector(Controller.Rigidbody.velocity)}");
        }
    }
}