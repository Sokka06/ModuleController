using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demos.Vehicle
{
    /// <summary>
    /// Simple downforce.
    /// </summary>
    public class VehicleDownforceModule : AbstractVehicleModule
    {
        [Header("Downforce")]
        public float Coefficient = 0.4f;
        public float FrontalArea = 2f;

        private const float AIR_DENSITY = 1.2041f;
     
        public override void UpdateModule(float deltaTime)
        {
            if (!Enabled)
                return;
            
            var velocity = Controller.Velocity;
            var downforce = 0.5f * Coefficient * FrontalArea * AIR_DENSITY * velocity.sqrMagnitude;
            Controller.Rigidbody.AddForce(-Controller.Transform.up * downforce);
        }
    }
}
