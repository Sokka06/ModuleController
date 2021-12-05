using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sokka06.ModuleController
{
    /// <summary>
    /// Implement this interface to make class control Modules.
    /// </summary>
    /// <typeparam name="TController">Your class that controls Modules.</typeparam>
    /// <typeparam name="TModule">Your abstract Module class that implements IModule interface.</typeparam>
    public interface IModuleController<TController, TModule>
        where TController : IModuleController<TController, TModule>
        where TModule : IModule<TController, TModule>
    {
        /// <summary>
        /// This controller's Modules.
        /// </summary>
        public List<TModule> Modules { get; }

        /// <summary>
        /// Setups modules, e.g. populate Modules list with FindModules and call SetupModule on them.
        /// </summary>
        public void SetupModules();

        /// <summary>
        /// Finds all modules. Should only be called once to populate Modules list.
        /// </summary>
        /// <returns></returns>
        public abstract TModule[] FindModules();

        /// <summary>
        /// Gets first instance of the given Module type.
        /// </summary>
        /// <typeparam name="T">Module type you want to find.</typeparam>
        /// <returns></returns>
        public T GetModule<T>() where T : TModule;

        /// <summary>
        /// Gets all Modules of given type and sets them to the supplied modules array.
        /// </summary>
        /// <typeparam name="T">Module type you want to find.</typeparam>
        /// <returns>Count of found modules.</returns>
        public int GetModules<T>(ref T[] modules) where T : TModule;
    }
}

