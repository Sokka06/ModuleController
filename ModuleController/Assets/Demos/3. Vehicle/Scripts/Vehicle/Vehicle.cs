using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demos.Vehicle
{
    public class Vehicle : MonoBehaviour
    {
        public VehicleController Controller;
        public AbstractDriver Driver { get; private set; }

        public event Action onDriverChanged;

        private void OnValidate()
        {
            if (Controller == null)
                Controller = GetComponentInChildren<VehicleController>();
        }

        public void AssignDriver(AbstractDriver driver)
        {
            Driver = driver;
            onDriverChanged?.Invoke();
        }
    }
}