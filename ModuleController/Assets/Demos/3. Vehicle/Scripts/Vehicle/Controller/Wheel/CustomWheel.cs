using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Demos.Vehicle
{
    public struct WheelGroundData
    {
        public bool HasGround;
        public WheelHit Hit;
        public Vector3 SidewaysDir;
        public Vector3 ForwardDir;
    }
    
    [RequireComponent(typeof(WheelCollider))]
    public class CustomWheel : MonoBehaviour
    {
        public WheelCollider Collider;

        [Space] 
        public float Stiffness = 1f;
        public AbstractFrictionModel LateralFrictionModel;
        
        // Used to reduce divisions.
        private float _inverseInertia;
        private float _inverseRadius;
        
        public float Inertia { get; private set; }
        public float Mass
        {
            get => Collider.mass;
            private set
            {
                Collider.mass = value;
                PreCalculate();
            }
        }

        public float Radius
        {
            get => Collider.radius;
            private set
            {
                Collider.radius = value;
                PreCalculate();
            }
        }
        
        public float SteerAngle
        {
            get => Collider.steerAngle;
            set => Collider.steerAngle = value;
        }
        
        /// <summary>
        /// Slip ratio from -1f = locked, 0f = rolling at same speed as ground to 1f+ rolling faster than the ground.
        /// </summary>
        public float SlipRatio { get; private set; }

        /// <summary>
        /// Slip angle in degrees from -90f to 90f.
        /// </summary>
        public float SlipAngle { get; private set; }

        public Vector3 Position { get; private set; }
        public Quaternion Orientation { get; private set; }
        /// <summary>
        /// Local velocity of the wheel. Notice: X and Y are switched, so X is for Longitudinal/forward speed and Y is for Lateral/sideways speed.
        /// </summary>
        public Vector2 LocalVelocity { get; private set; }
        /// <summary>
        /// Angular velocity of the wheel in radians.
        /// </summary>
        public float AngularVelocity { get; private set; }
        
        public float LongitudinalFrictionForce { get; private set; }
        public float LateralFrictionForce { get; private set; }
        
        public Rigidbody Rigidbody => Collider.attachedRigidbody;
        public WheelGroundData GroundData { get; private set; }

        private void OnValidate()
        {
            if (Collider == null)
                Collider = GetComponent<WheelCollider>();
        
            if (Collider == null)
                return;

            var forwardFriction = Collider.forwardFriction;
            forwardFriction.stiffness = 0f;
            Collider.forwardFriction = forwardFriction;
        
            var sidewaysFriction = Collider.sidewaysFriction;
            sidewaysFriction.stiffness = 0f;
            Collider.sidewaysFriction = sidewaysFriction;
        }

        private void Awake()
        {
            GroundData = new WheelGroundData();

            PreCalculate();
        }

        private void PreCalculate()
        {
            Inertia = Mass * Radius * Radius * 0.5f;
            
            _inverseInertia = 1f / Inertia;
            _inverseRadius = 1f / Radius;
        }

        public void UpdateWheel(float deltaTime)
        {
            // Weird bug fix
            Collider.motorTorque = 0.00001f;
            //Collider.brakeTorque = 0.00001f;

            LocalVelocity = GetLocalVelocity();
            SlipRatio = GetSlipRatio();
            SlipAngle = GetSlipAngle();

            // Update Wheel Data
            Collider.GetWorldPose(out var pos, out var rot);
            Position = pos;
            Orientation = rot;

            // Ground Data
            var groundData = new WheelGroundData
            {
                HasGround = Collider.GetGroundHit(out var wheelHit),
                Hit = wheelHit,
                SidewaysDir = Vector3.ProjectOnPlane(GetRight(), GroundData.Hit.normal),
                ForwardDir = Vector3.ProjectOnPlane(GetForward(), GroundData.Hit.normal)
            };
            GroundData = groundData;
            
            // Only Lateral friction is used.
            var frictionForce = Vector3.zero;
            var lateralForce = 0f;

            if (GroundData.HasGround)
            {
                if (LateralFrictionModel != null)
                    LateralFrictionModel.GetLateral(GroundData.Hit.force, SlipAngle, deltaTime, out lateralForce);

                frictionForce += GroundData.SidewaysDir * -lateralForce;
                
                Rigidbody.AddForceAtPosition(frictionForce * Stiffness, wheelHit.point);
            }

            LateralFrictionForce = lateralForce;
        }

        /// <summary>
        /// Calculates local velocity.
        /// </summary>
        /// <returns></returns>
        private Vector2 GetLocalVelocity()
        {
            //velocity at ground Patch
            var pointVelocity = Rigidbody.GetPointVelocity(GroundData.Hit.point);

            var forward = Vector3.ProjectOnPlane(GetForward(), GroundData.Hit.normal);
            var longitudinalVelocity = Vector3.Dot(pointVelocity, forward);

            var sideways = Vector3.ProjectOnPlane(GetRight(), GroundData.Hit.normal);
            var lateralVelocity = Vector3.Dot(pointVelocity, sideways);

            return new Vector2(longitudinalVelocity, lateralVelocity);
        }

        /// <summary>
        /// Calculates Slip Ratio
        /// </summary>
        /// <returns></returns>
        private float GetSlipRatio()
        {
            var slipRatio = 0f;
            
            if (!GroundData.HasGround || Mathf.Abs(LocalVelocity.x) <= Mathf.Epsilon)
                return slipRatio;
            
            var longitudinalSpeed = LocalVelocity.x;
            var wheelVelocity = AngularVelocity * Radius;
            
            // just cause 4 formula
            /*var isStopped = Mathf.Abs(AngularVelocity) < Mathf.Epsilon;
            var slideSign = isStopped ? Mathf.Sign(LocalVelocity.x) : Mathf.Sign(AngularVelocity);
            var slipSpeed = (wheelVelocity - LocalVelocity.x) * slideSign;
            slipRatio = slipSpeed / Mathf.Abs(LocalVelocity.x);*/

            // wikipedia formula
            //slipRatio = (wheelVelocity / longitudinalSpeed) - 1f;

            //alternative formula
            slipRatio = (wheelVelocity - longitudinalSpeed) / longitudinalSpeed;
            
            // formula
            /*
            if (longitudinalSpeed == 0 && wheelVelocity == 0) { return 0f; }//no slip present
            var a = Mathf.Max(longitudinalSpeed, wheelVelocity);
            var b = Mathf.Min(longitudinalSpeed, wheelVelocity);
            slipRatio = (a - b) / Mathf.Abs(a);
            slipRatio = Mathf.Clamp(slipRatio, 0, 1);*/
            
            return slipRatio;
        }
        
        /// <summary>
        /// Calculates Slip angle.
        /// </summary>
        /// <returns></returns>
        private float GetSlipAngle()
        {
            var slipAngle = 0f;

            if (!GroundData.HasGround || Mathf.Abs(LocalVelocity.y) <= 0.01f)
                return slipAngle;

            if (LocalVelocity.sqrMagnitude < 0.1f)
            {
                // Alternative slip angle calculation when velocity is low.
                slipAngle = Mathf.Clamp(LocalVelocity.y, -1f, 1f) * 90f;
            }
            else
            {
                // Normal slip angle calculation.
                slipAngle = Mathf.Atan2(LocalVelocity.y, Mathf.Abs(LocalVelocity.x)) * Mathf.Rad2Deg;
            }
            
            //slipAngle = Mathf.Atan2(LocalVelocity.y, Mathf.Abs(LocalVelocity.x)) * Mathf.Rad2Deg;

            return slipAngle;
        }

        public Vector3 GetRight()
        {
            return Orientation * Vector3.right;
        }

        public Vector3 GetUp()
        {
            return Orientation * Vector3.up;
        }

        public Vector3 GetForward()
        {
            return Orientation * Vector3.forward;
        }

        private void OnDrawGizmosSelected()
        {
            if (GroundData.HasGround)
            {
                var deltaTime = Time.fixedDeltaTime;
                Gizmos.color = Color.blue;
                Gizmos.DrawRay(GroundData.Hit.point, GroundData.ForwardDir * LongitudinalFrictionForce * deltaTime);
                Gizmos.color = Color.red;
                Gizmos.DrawRay(GroundData.Hit.point, GroundData.SidewaysDir * LateralFrictionForce * deltaTime);
            }
            
            Handles.Label(transform.position, $"{LocalVelocity.sqrMagnitude}");
        }

        [ContextMenu("Calculate suspension values")]
        public void SetupSuspension()
        {
            // Spring Rate = vehicle mass / number of wheels * 2 * gravity / suspension distance
            // Damper Rate = Spring Rate / 20
            var gravity = Physics.gravity.magnitude;
            var mass = Rigidbody.mass;
            var wheelCount = Rigidbody.GetComponentsInChildren<WheelCollider>().Length;
            
            var springRate = mass / wheelCount * 2f * gravity / Collider.suspensionDistance;
            var springDamper = springRate / 20f;

            var springJoint = Collider.suspensionSpring;
            springJoint.spring = springRate;
            springJoint.damper = springDamper;
            Collider.suspensionSpring = springJoint;
        }
    }
}