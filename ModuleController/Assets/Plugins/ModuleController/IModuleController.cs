using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sokka06.ModuleController
{
    /// <summary>
    /// Implement this interface to make class control modules.
    /// </summary>
    /// <typeparam name="TController">Your class that controls modules.</typeparam>
    /// <typeparam name="TModule">Your abstract module class that implements IModule interface.</typeparam>
    public interface IModuleController<TController, TModule>
        where TController : IModuleController<TController, TModule>
        where TModule : IModule<TController, TModule>
    {
        /// <summary>
        /// This controller's modules.
        /// </summary>
        public List<TModule> Modules { get; }

        /// <summary>
        /// Setups modules, e.g. populate "Modules" list with FindModules and call SetupModule on them.
        /// </summary>
        public void SetupModules();

        /// <summary>
        /// Finds all modules.
        /// </summary>
        /// <returns></returns>
        public abstract TModule[] FindModules();

        /// <summary>
        /// Gets specific module for given type.
        /// </summary>
        /// <typeparam name="T">Module type you want to get.</typeparam>
        /// <returns></returns>
        public T GetModule<T>() where T : TModule;
    }
}

