using System.Collections;
using System.Collections.Generic;
using Demos.Vehicle;
using Sokka06.ModuleController;
using UnityEngine;

namespace Demos.Vehicle
{
    public abstract class AbstractAssistModule : ModuleBehaviour<VehicleAssistsModule, AbstractAssistModule>
    {
        public override void UpdateModule(float deltaTime)
        {
        
        }
    }
}

