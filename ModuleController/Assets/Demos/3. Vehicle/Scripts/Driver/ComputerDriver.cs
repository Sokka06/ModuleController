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

        private void Awake()
        {
        }

        public override void AssignVehicle(Vehicle vehicle)
        {
            base.AssignVehicle(vehicle);
            
        }
    }
}