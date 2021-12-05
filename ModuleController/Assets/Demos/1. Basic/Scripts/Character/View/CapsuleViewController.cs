using System.Collections;
using System.Collections.Generic;
using Sokka06.ModuleController;
using UnityEngine;

namespace Demos.Demo1
{
    public class CapsuleViewController : MonoBehaviour, IModuleController<CapsuleViewController, AbstractViewModule>
    {
        public CharacterModuleController Character;
        
        public List<AbstractViewModule> Modules { get; protected set; }
        
        private void Awake()
        {
            SetupModules();
        }

        private void LateUpdate()
        {
            var deltaTime = Time.deltaTime;

            for (int i = 0; i < Modules.Count; i++)
            {
                Modules[i].UpdateModule(deltaTime);
            }
        }
        
        #region ModuleController
        public void SetupModules()
        {
            Modules = new List<AbstractViewModule>(FindModules());
            
            for (int i = 0; i < Modules.Count; i++)
            {
                Modules[i].SetupModule(this);
            }
        }

        public AbstractViewModule[] FindModules()
        {
            // Define where Modules are found.
            // In this case they're simply placed under this same GameObject and found with GetComponents.
            // You can also, for example, use GetComponentsInChildren if they're placed in a child object or another list that is manually populated in inspector
            
            return GetComponents<AbstractViewModule>();
        }

        public T GetModule<T>() where T : AbstractViewModule
        {
            // Finds the module for you and returns it. Returns null if module is not on the list.
            for (int i = 0; i < Modules.Count; i++)
            {
                if (Modules[i] is T module)
                    return module;
            }

            return null;
        }

        public int GetModules<T>(ref T[] modules) where T : AbstractViewModule
        {
            // Not needed this for this demo.
            throw new System.NotImplementedException();
        }

        #endregion
    }
}