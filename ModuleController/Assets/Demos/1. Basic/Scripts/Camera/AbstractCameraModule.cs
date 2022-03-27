using System.Collections;
using System.Collections.Generic;
using Sokka06.ModuleController;
using UnityEngine;

namespace Demos.Demo1
{
    public abstract class AbstractCameraModule : ModuleBehaviour<FirstPersonCameraController, AbstractCameraModule>
    {
        public override void SetupModule(FirstPersonCameraController controller)
        {
            base.SetupModule(controller);
        }
    }
}
