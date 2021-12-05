using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sokka06.ModuleController
{
    /// <summary>
    /// Implement this interface in your abstract module class to make it a module for a controller.
    /// </summary>
    /// <typeparam name="TController">Your class that controls these modules.</typeparam>
    /// <typeparam name="TModule">Your abstract module class.</typeparam>
    public interface IModule<TController, TModule> 
        where TController : IModuleController<TController, TModule>
        where TModule : IModule<TController, TModule>
    {
        public TController Controller { get; }

        public void SetupModule(TController controller);

        public void UpdateModule(float deltaTime);
    }
}

