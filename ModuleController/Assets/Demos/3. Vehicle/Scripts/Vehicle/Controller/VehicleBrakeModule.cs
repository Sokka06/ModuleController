using System.Collections;
using System.Collections.Generic;
using Demos.Vehicle;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Demos.Vehicle
{
    public class VehicleBrakeModule : AbstractVehicleModule
    {
        [Header("Steer Module")]
        public List<WheelCollider> BrakedWheels;

        public float BrakeFactor = 2f;

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
            var input = _gearboxModule.Gearbox.GetRatio() >= 0f ? _inputModule.Inputs.Brake : 0f;
            
            if (!Enabled || !(input > 0f))
                return;
            
            // simple drag formula.
            //velocity = velocity * ( 1 - deltaTime * drag);

            for (int i = 0; i < BrakedWheels.Count; i++)
            {
                var isGrounded = BrakedWheels[i].GetGroundHit(out var wheelHit);
                if (!isGrounded)
                    continue;

                var velocity = Controller.PointVelocity(wheelHit.point);
                
                var load = BrakedWheels[i].sprungMass;
                var force = Vector3.ClampMagnitude(-Vector3.ProjectOnPlane(velocity, wheelHit.normal), 1f) * load * BrakeFactor * input;

                Controller.AddVelocity(force, wheelHit.point);
            }
        }

        /// <summary>
        /// Formula no.1
        /// </summary>
        /// <param name="velocity"></param>
        /// <param name="deltaTime"></param>
        /// <returns></returns>
        private Vector3 GetBrakeForceDrag(Vector3 velocity, float deltaTime)
        {
            var factor = BrakeFactor;
            var v = velocity * factor;
            var c = (1 - deltaTime * factor);
            var force = v / c;
            
            return -force;
        }
        
        /// <summary>
        /// Formula no.2
        /// </summary>
        /// <param name="velocity"></param>
        /// <param name="mass"></param>
        /// <returns></returns>
        private Vector3 GetBrakeForce(Vector3 velocity, float mass)
        {
            //F = 0.5f * mass * velocity^2
            var force = 0.5f * mass * Mathf.Pow(velocity.magnitude, 2f);
            
            return -velocity.normalized * force;
        }
    }
}