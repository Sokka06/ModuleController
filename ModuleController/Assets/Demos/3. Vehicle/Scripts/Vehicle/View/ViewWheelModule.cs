using System;
using System.Collections;
using System.Collections.Generic;
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

        private void OnValidate()
        {
            if (Root == null)
                Root = transform.GetChild(0);
        }

        public override void SetupModule(VehicleViewController controller)
        {
            base.SetupModule(controller);

            _offset = Root.localPosition - Collider.transform.localPosition;
        }

        public override void UpdateModule(float deltaTime)
        {
            if (!Enabled)
                return;

            Collider.GetWorldPose(out var position, out var rotation);
            Root.SetPositionAndRotation(position + transform.TransformVector(_offset), rotation);
            //Root.position = position + Root.TransformVector(_offset);
            //Root.rotation = rotation;
        }

    }
}