using System.Collections;
using System.Collections.Generic;
using Demos.Vehicle;
using Sokka06.ModuleController;
using UnityEngine;

namespace Demos.Vehicle
{
    public class VehicleAssistsModule : AbstractVehicleModule, IModuleController<VehicleAssistsModule, AbstractAssistModule>
    {
        public List<AbstractAssistModule> Modules { get; private set; }

        public override void SetupModule(VehicleController controller)
        {
            base.SetupModule(controller);
            
            SetupModules();
        }

        public override void UpdateModule(float deltaTime)
        {
            if (!Enabled)
                return;

            for (int i = 0; i < Modules.Count; i++)
            {
                Modules[i].UpdateModule(deltaTime);
            }
        }

        public void SetupModules()
        {
            Modules = new List<AbstractAssistModule>(FindModules());

            for (int i = 0; i < Modules.Count; i++)
            {
                Modules[i].SetupModule(this);
            }
        }

        public AbstractAssistModule[] FindModules()
        {
            return GetComponentsInChildren<AbstractAssistModule>();
        }

        public T GetModule<T>() where T : AbstractAssistModule
        {
            for (int i = 0; i < Modules.Count; i++)
            {
                if (Modules[i] is T module)
                    return module;
            }

            return null;
        }
    }
}

