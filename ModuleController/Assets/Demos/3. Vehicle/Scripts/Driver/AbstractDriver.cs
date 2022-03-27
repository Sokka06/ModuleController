using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demos.Vehicle
{
    public class AbstractDriver : MonoBehaviour
    {
        [Header("Driver")]
        public string Name;
    
        public Vehicle Vehicle { get; private set; }
        public event Action onVehicleChanged;

        public virtual void Setup(Vehicle vehicle)
        {
            AssignVehicle(vehicle);
        }

        public virtual void AssignVehicle(Vehicle vehicle)
        {
            Vehicle = vehicle;
            Vehicle.AssignDriver(this);
            
            onVehicleChanged?.Invoke();
        }
    }
}