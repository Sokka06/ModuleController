using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sokka06.ModuleController
{
    /// <summary>
    /// Basic Module Controller with boilerplate code included.
    /// </summary>
    public abstract class ModuleControllerBehaviour<TController, TModule> : MonoBehaviour, IModuleController<TController, TModule> 
        where TController : MonoBehaviour, IModuleController<TController, TModule> 
        where TModule : MonoBehaviour, IModule<TController, TModule>
    {
        public List<TModule> Modules { get; protected set; }

        public virtual void SetupModules()
        {
            Modules = new List<TModule>(FindModules());
            
            for (int i = 0; i < Modules.Count; i++)
            {
                Modules[i].SetupModule(this as TController);
            }
        }

        /// <summary>
        /// Updates all modules.
        /// </summary>
        /// <param name="deltaTime"></param>
        public virtual void UpdateModules(float deltaTime)
        {
            for (int i = 0; i < Modules.Count; i++)
            {
                Modules[i].UpdateModule(deltaTime);
            }
        }

        public virtual TModule[] FindModules()
        {
            // Define where Modules are found.
            // In this case they're simply placed under this same GameObject and found with GetComponents.
            // You can also, for example, use GetComponentsInChildren if they're placed in a child object or another list that is manually populated in inspector
            
            return GetComponents<TModule>();
        }

        public virtual T GetModule<T>() where T : TModule
        {
            // Finds the module for you and returns it. Returns null if module is not on the list.
            for (int i = 0; i < Modules.Count; i++)
            {
                if (Modules[i] is T module)
                    return module;
            }

            return null;
        }
    }
}