using System;
using System.Collections;
using System.Collections.Generic;
using Demos;
using UnityEngine;

namespace Demos.Common
{
    public class ModuleLister : MonoBehaviour
    {
        public ModuleToggle ModuleTogglePrefab;
        public CharacterModuleController CharacterModuleController;
        public Transform Container;

        private void Start()
        {
            for (int i = 0; i < CharacterModuleController.Modules.Count; i++)
            {
                var moduleToggle = Instantiate(ModuleTogglePrefab, Container);
                moduleToggle.Bind(CharacterModuleController.Modules[i]);
            }
        }
    }
}