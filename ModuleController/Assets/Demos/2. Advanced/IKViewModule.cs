/*using System;
using System.Collections;
using System.Collections.Generic;
using Sokka06.ModuleController;
using UnityEngine;

namespace Demos.Demo2
{
    public class IKViewModule : AbstractViewModule, IModuleController<IKViewModule, AbstractIKModule>
    {
        public AnimatorListener AnimatorListener;


        public List<AbstractIKModule> Modules { get; private set; }

        public override void SetupModule(CharacterViewController controller)
        {
            base.SetupModule(controller);
            
            SetupModules();
            
            AnimatorListener.onAnimatorIK += OnIKUpdate;
        }

        private void OnDestroy()
        {
            AnimatorListener.onAnimatorIK -= OnIKUpdate;
        }

        private void OnIKUpdate(float deltaTime, int layerIndex)
        {
            UpdateModule(deltaTime);
        }

        public override void UpdateModule(float deltaTime)
        {
            base.UpdateModule(deltaTime);
            
            for (int i = 0; i < Modules.Count; i++)
            {
                Modules[i].UpdateModule(deltaTime);
            }
        }

        public void SetupModules()
        {
            Modules = new List<AbstractIKModule>(FindModules());
            for (int i = 0; i < Modules.Count; i++)
            {
                Modules[i].SetupModule(this);
            }
        }

        public AbstractIKModule[] FindModules()
        {
            return GetComponentsInChildren<AbstractIKModule>();
        }

        public T GetModule<T>() where T : AbstractIKModule
        {
            for (int i = 0; i < Modules.Count; i++)
            {
                if (Modules[i] is T module)
                    return module;
            }

            return null;
        }
    }
}*/