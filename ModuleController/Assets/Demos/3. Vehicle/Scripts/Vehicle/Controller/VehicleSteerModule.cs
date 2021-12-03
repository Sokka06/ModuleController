using System.Collections;
using System.Collections.Generic;
using Demos.Vehicle;
using UnityEngine;
using UnityEngine.InputSystem;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Demos.Vehicle
{
    public class VehicleSteerModule : AbstractVehicleModule
    {
        [Header("Steer Module")]
        public List<WheelCollider> SteeredWheels;
        [Range(0f, 90f)]
        public float SteerAngle = 30f;
        public AnimationCurve SteerCurve = AnimationCurve.Linear(0f, 1f, 1f, 0.5f);
        [Tooltip("Use Ackermann steering to adjust steering angle.")]
        public bool Ackermann = true;

        private VehicleInputModule _inputModule;
        private VehicleDriveModule _driveModule;

        public override void SetupModule(VehicleController controller)
        {
            base.SetupModule(controller);

            _inputModule = Controller.GetModule<VehicleInputModule>();
            _driveModule = Controller.GetModule<VehicleDriveModule>();
        }

        public override void UpdateModule(float deltaTime)
        {
            if (!Enabled)
                return;
            
            var forwardSpeed = Vector3.Dot(Controller.Rigidbody.velocity, Controller.Transform.forward);
            var speedFactor = Mathf.Abs(forwardSpeed) / _driveModule.Speed;
            var steerAngle = SteerAngle * SteerCurve.Evaluate(speedFactor) * _inputModule.Inputs.Steer;
            
            // For Ackermann
            var frontAxle = Controller.Wheels[0].transform.localPosition;
            frontAxle.x = 0f;
            
            var rearAxle = Controller.Wheels[2].transform.localPosition;
            rearAxle.x = 0f;

            var wheelBase = Vector3.Distance(frontAxle, rearAxle);
            var axleLength = Vector3.Distance(SteeredWheels[0].transform.localPosition,
                SteeredWheels[1].transform.localPosition);

            for (int i = 0; i < SteeredWheels.Count; i++)
            {
                if (Ackermann)
                {
                    steerAngle = GetAckermann(steerAngle, wheelBase,
                        axleLength, SteeredWheels[i].transform.localPosition.x < 0f ? -1 : 1);
                }
                
                SteeredWheels[i].steerAngle = steerAngle;
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="steerAngle">Target steer angle</param>
        /// <param name="wheelBase">Distance from back wheel to front wheel</param>
        /// <param name="width">Width of the vehicle or axle length</param>
        /// <param name="side">-1 = left wheel and 1 = right wheel. 0 = returns nothing.</param>
        /// <returns></returns>
        public static float GetAckermann(float steerAngle, float wheelBase, float width, int side = -1)
        {
            if (steerAngle == 0.0f || side == 0) 
                return 0.0f;
            
            var turnRad = wheelBase / Mathf.Tan(Mathf.Deg2Rad * steerAngle);
            
            return Mathf.Rad2Deg * Mathf.Atan(wheelBase / (turnRad + -side * width));
        }
        
#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            var color = Color.yellow;
            color.a *= 0.5f;
            Handles.color = color;
            
            for (int i = 0; i < SteeredWheels.Count; i++)
            {
                var origin = SteeredWheels[i].transform.position;
                var up = SteeredWheels[i].transform.up;
                var forward = SteeredWheels[i].transform.forward;
                var left = Quaternion.AngleAxis(-SteerAngle, up) * forward;
                var right = Quaternion.AngleAxis(SteerAngle, up) * forward;
                
                // Draw left line
                Handles.DrawLine(origin, origin + left * SteeredWheels[i].radius);
                // Draw arc
                Handles.DrawWireArc(origin, up, left, SteerAngle * 2f, SteeredWheels[i].radius);
                // Draw right line
                Handles.DrawLine(origin, origin + right * SteeredWheels[i].radius);
                
                // Draw current steer angle.
                Handles.DrawDottedLine(origin, origin + Quaternion.AngleAxis(SteeredWheels[i].steerAngle, up) * forward * SteeredWheels[i].radius, 1f);
            }
        }
#endif
    }
}