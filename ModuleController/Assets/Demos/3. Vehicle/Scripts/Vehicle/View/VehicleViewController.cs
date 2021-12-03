using System;
using System.Collections;
using System.Collections.Generic;
using Sokka06.ModuleController;
using UnityEngine;

namespace Demos.Vehicle
{
    public class VehicleViewController : ModuleControllerBehaviour<VehicleViewController, AbstractViewModule>
    {
        private void Awake()
        {
            SetupModules();
        }

        private void LateUpdate()
        {
            UpdateModules(Time.deltaTime);
        }

        public override AbstractViewModule[] FindModules()
        {
            // Look for modules under this gameObject.
            return GetComponentsInChildren<AbstractViewModule>();
        }
    }
}