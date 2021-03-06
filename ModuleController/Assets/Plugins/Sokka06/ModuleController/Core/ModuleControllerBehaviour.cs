using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sokka06.ModuleController
{
    /// <summary>
    /// Basic Module Controller with boilerplate code included.
    /// </summary>
    [DefaultExecutionOrder(-1)]
    public abstract class ModuleControllerBehaviour<TController, TModule> : MonoBehaviour, IModuleController<TController, TModule> 
        where TController : MonoBehaviour, IModuleController<TController, TModule> 
        where TModule : MonoBehaviour, IModule<TController, TModule>
    {
        public List<TModule> Modules { get; protected set; } = new List<TModule>();

        protected virtual void Awake()
        {
            Modules = new List<TModule>(FindModules());
        }

        public virtual void SetupModules()
        {
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
        
        public int GetModules<T>(ref T[] modules) where T : TModule
        {
            var count = 0;
            for (int i = 0; i < Modules.Count; i++)
            {
                if (Modules[i] is T module)
                {
                    modules[count] = module;
                    count++;
                }
                
                if (count >= modules.Length)
                    break;
            }
            
            return count;
        }
    }
}