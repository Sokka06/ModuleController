using System;
using System.Collections;
using System.Collections.Generic;
using Sokka06.ModuleController;
using UnityEngine;

namespace Demos.Vehicle
{
    public class AbstractAIModule : ModuleBehaviour<DriverAIController, AbstractAIModule>
    {
        public virtual void AssignVehicle(Vehicle vehicle)
        {
            
        }

        public virtual void UpdateState(float deltaTime)
        {
            
        }

        public override void UpdateModule(float deltaTime)
        {
            
        }
    }
}

