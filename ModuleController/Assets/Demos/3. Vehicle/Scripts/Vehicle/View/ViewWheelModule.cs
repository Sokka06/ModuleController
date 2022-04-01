using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace Demos.Vehicle
{
    /// <summary>
    /// Aligns wheel mesh to Wheel Collider.
    /// </summary>
    public class ViewWheelModule : AbstractViewModule
    {
        [Header("Wheel View")]
        public WheelCollider Collider;
        public Transform Root;

        private Vector3 _offset;
        private float _angularVelocity;
        private Quaternion _rollRotation;

        private void OnValidate()
        {
            if (Root == null)
                Root = transform.GetChild(0);
        }

        public override void SetupModule(VehicleViewController controller)
        {
            base.SetupModule(controller);

            _offset = Root.localPosition - Collider.transform.localPosition;
            _rollRotation = Quaternion.identity;
        }

        public override void UpdateModule(float deltaTime)
        {
            if (!Enabled)
                return;

            Collider.GetWorldPose(out var position, out var rotation);
            Root.SetPositionAndRotation(position + transform.TransformVector(_offset), rotation);

            if (Collider.isGrounded)
            {
                // Longitudinal friction is not used to rotate the wheel,
                // so we have to calculate Angular velocity from forward velocity in order to roll the wheel along the ground.
                var forwardVelocity = Vector3.Dot(Collider.attachedRigidbody.velocity,
                    Collider.attachedRigidbody.transform.forward);
                _angularVelocity = forwardVelocity / Collider.radius;
            }
            else
            {
                // Apply angular drag when not on ground.
                const float dragCoefficient = 0.5f;
                _angularVelocity *= 1f - deltaTime * dragCoefficient;
            }
            
            _rollRotation *= Quaternion.AngleAxis(_angularVelocity * Mathf.Rad2Deg * deltaTime, Vector3.right);
            Root.localRotation *= _rollRotation;

            //Root.position = position + Root.TransformVector(_offset);
            //Root.rotation = rotation;
        }

        private void OnDrawGizmos()
        {
            //Gizmos.DrawRay(Root.position, Root.forward);
            //Handles.Label(Collider.transform.position, $"Angular Velocity: {_angularVelocity}");
        }
    }
}