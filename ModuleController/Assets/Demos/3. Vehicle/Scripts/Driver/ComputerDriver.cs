using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demos.Vehicle
{
    /// <summary>
    /// 
    /// </summary>
    public class ComputerDriver : AbstractDriver
    {
        public RaceAI RaceAI;

        public override void Setup(Vehicle vehicle)
        {
            base.Setup(vehicle);
            
            RaceAI.Setup(vehicle);
        }

        private void Awake()
        {
        }

        public override void AssignVehicle(Vehicle vehicle)
        {
            base.AssignVehicle(vehicle);
            
        }
    }
}