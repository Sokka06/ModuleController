using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demos
{
    /// <summary>
    /// Adds gravity to Character.
    /// </summary>
    public class CharacterGravityModule : AbstractCharacterModule
    {
        [Header("Gravity")]
        public float Gravity = -20f;
        
        public Vector3 Up { get; private set; }

        public override void SetupModule(CharacterModuleController controller)
        {
            base.SetupModule(controller);
            
            SetGravityUp(Vector3.up);
        }

        public override void UpdateModule(float deltaTime)
        {
            if (!Enabled)
                return;

            Controller.AddVelocity(Up * Gravity * deltaTime);
        }

        /// <summary>
        /// Sets gravity up direction.
        /// </summary>
        /// <param name="up"></param>
        public void SetGravityUp(Vector3 up)
        {
            Up = up;
        }
    }
}