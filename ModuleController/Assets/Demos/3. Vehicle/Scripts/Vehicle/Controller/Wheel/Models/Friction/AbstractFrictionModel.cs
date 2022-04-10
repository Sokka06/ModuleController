using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demos.Vehicle
{
    public abstract class AbstractFrictionModel : ScriptableObject
    {
        protected const string MENU_NAME = "Vehicle/Friction Model/";
        protected const string FILE_NAME = "Friction Model";
        
        /// <summary>
        /// Returns Longitudinal/forward friction force.
        /// </summary>
        /// <param name="wheel"></param>
        /// <param name="deltaTime"></param>
        /// <param name="longitudinal"></param>
        public virtual void GetLongitudinal(float load, float slip, float deltaTime, out float longitudinal)
        {
            longitudinal = 0f;
        }
        
        /// <summary>
        /// Returns Lateral/sideways friction force.
        /// </summary>
        /// <param name="wheel"></param>
        /// <param name="deltaTime"></param>
        /// <param name="lateral"></param>
        public virtual void GetLateral(float load, float slip, float deltaTime, out float lateral)
        {
            lateral = 0f;
        }
    }
}