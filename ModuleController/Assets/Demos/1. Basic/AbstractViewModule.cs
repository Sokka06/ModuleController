using System.Collections;
using System.Collections.Generic;
using Sokka06.ModuleController;
using UnityEngine;

namespace Demos.Demo1
{
    public abstract class AbstractViewModule : MonoBehaviour, IModule<CapsuleViewController, AbstractViewModule>
    {
        public bool Enabled = true;
        
        public CapsuleViewController Controller { get; private set; }

        public virtual void SetupModule(CapsuleViewController controller)
        {
            Controller = controller;
        }

        public abstract void UpdateModule(float deltaTime);
    }
}

