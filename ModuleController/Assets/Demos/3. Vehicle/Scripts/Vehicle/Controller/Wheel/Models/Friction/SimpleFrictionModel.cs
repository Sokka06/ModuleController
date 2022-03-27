using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demos.Vehicle
{
    /// <summary>
    /// A friction model that uses simple curves to simulate friction at different amounts of slip.
    /// </summary>
    [CreateAssetMenu(menuName = MENU_NAME + MODEL_NAME, fileName = MODEL_NAME + " " + FILE_NAME)]
    public class SimpleFrictionModel : AbstractFrictionModel
    {
        [Header(MODEL_NAME)]
        public AnimationCurve LongitudinalCurve = AnimationCurve.Linear(0f, 1f, 1f, 0.2f);

        [Space]
        public AnimationCurve LateralCurve = AnimationCurve.Linear(0f, 1f, 1f, 0.2f);

        protected const string MODEL_NAME = "Simple";

        public override void GetLongitudinal(CustomWheel wheel, float deltaTime, out float longitudinal)
        {
            base.GetLongitudinal(wheel, deltaTime, out longitudinal);
        }

        public override void GetLateral(CustomWheel wheel, float deltaTime, out float lateral)
        {
            // Slip angle normalized to -1 to 1f.
            var slip = wheel.SlipAngle / 90f;
            var velocity = wheel.Rigidbody.GetPointVelocity(wheel.GroundData.Hit.point);
            var right = Vector3.ProjectOnPlane(wheel.GetRight(), wheel.GroundData.Hit.normal);
            var lateralVelocity = Vector3.Dot(velocity, right);
            var frictionRatio = Mathf.Abs(Vector3.Dot(velocity.normalized, right));

            var load = wheel.Collider.sprungMass;
            var mass = wheel.Rigidbody.mass;
            var factor = load / mass;

            lateral = -lateralVelocity * load * factor * LateralCurve.Evaluate(frictionRatio) / deltaTime;
        }
    }
}