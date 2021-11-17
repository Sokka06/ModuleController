using System.Collections;
using System.Collections.Generic;
using Sokka06.ModuleController;
using UnityEngine;

namespace Demos
{
    /// <summary>
    /// Base class for all Character modules.
    /// </summary>
    public abstract class AbstractCharacterModule : MonoBehaviour, IModule<CharacterModuleController, AbstractCharacterModule>
    {
        public bool Enabled = true;
        
        public CharacterModuleController Controller { get; private set; }

        public virtual void SetupModule(CharacterModuleController controller)
        {
            Controller = controller;
        }

        public abstract void UpdateModule(float deltaTime);
    }
}