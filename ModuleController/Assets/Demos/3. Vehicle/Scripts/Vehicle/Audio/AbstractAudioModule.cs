using System;
using System.Collections;
using System.Collections.Generic;
using Sokka06.ModuleController;
using UnityEngine;

namespace Demos.Vehicle
{
    public class AbstractAudioModule : ModuleBehaviour<VehicleAudioController, AbstractAudioModule>
    {
        public AudioSource Source;

        protected virtual void OnValidate()
        {
            if (Source == null)
                Source = GetComponent<AudioSource>();
        }

        public override void UpdateModule(float deltaTime)
        {
            
        }
    }
}