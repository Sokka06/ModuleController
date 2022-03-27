using System;
using System.Collections;
using System.Collections.Generic;
using Sokka06.ModuleController;
using UnityEngine;

namespace Demos.Vehicle
{
    public class VehicleAudioController : ModuleControllerBehaviour<VehicleAudioController, AbstractAudioModule>
    {
        public VehicleController VehicleController;
        
        void OnValidate()
        {
            if (VehicleController == null)
                VehicleController = transform.root.GetComponentInChildren<VehicleController>();
        }
        
        private void Start()
        {
            SetupModules();
        }

        private void LateUpdate()
        {
            UpdateModules(Time.deltaTime);
        }

        public override AbstractAudioModule[] FindModules()
        {
            return GetComponentsInChildren<AbstractAudioModule>();
        }
    }
}