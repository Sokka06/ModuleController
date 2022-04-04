using System.Collections;
using System.Collections.Generic;
using Demos.Vehicle;
using Sokka06.ModuleController;
using UnityEngine;

namespace Demos.Vehicle
{
    public class AbstractDemoModule : ModuleBehaviour<RaceDemoController, AbstractDemoModule>
    {
        public GameObject Container;

        public override void SetupModule(RaceDemoController controller)
        {
            base.SetupModule(controller);
            
            Container.SetActive(false);
        }

        public override void UpdateModule(float deltaTime)
        {
        
        }

        public virtual void EnterModule()
        {
            Container.SetActive(true);
        }

        public virtual void ExitModule()
        {
            Container.SetActive(false);
        }
    }
}