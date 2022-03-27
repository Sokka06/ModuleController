using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demos.Vehicle
{
    /// <summary>
    /// Friction model based on the default Unity Wheel Collider friction.
    /// </summary>
    [CreateAssetMenu(menuName = MENU_NAME + MODEL_NAME, fileName = MODEL_NAME + " " + FILE_NAME)]
    public class DefaultFrictionModel : AbstractFrictionModel
    {
        [Header(MODEL_NAME)]
        public WheelFrictionCurve LongitudinalFriction;
        public WheelFrictionCurve LateralFriction;

        protected const string MODEL_NAME = "Default";


        public override void GetLongitudinal(CustomWheel wheel, float deltaTime, out float longitudinal)
        {
            base.GetLongitudinal(wheel, deltaTime, out longitudinal);
            
        }

        public override void GetLateral(CustomWheel wheel, float deltaTime, out float lateral)
        {
            var velocity = wheel.Rigidbody.GetPointVelocity(wheel.GroundData.Hit.point);
            var right = Vector3.ProjectOnPlane(wheel.GetRight(), wheel.GroundData.Hit.normal);
            var sidewaysVelocity = Vector3.Dot(velocity, right);

            lateral = 0f;
        }
    }
}