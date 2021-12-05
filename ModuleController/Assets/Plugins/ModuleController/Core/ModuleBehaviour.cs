using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sokka06.ModuleController
{
    /// <summary>
    /// Basic Module with boilerplate code.
    /// </summary>
    public abstract class ModuleBehaviour<TController, TModule> : MonoBehaviour, IModule<TController, TModule> 
        where TController : IModuleController<TController, TModule> 
        where TModule : IModule<TController, TModule>
    {
        public bool Enabled = true;
        
        public TController Controller { get; private set; }

        public virtual void SetupModule(TController controller)
        {
            Controller = controller;
        }

        public abstract void UpdateModule(float deltaTime);
    }
}