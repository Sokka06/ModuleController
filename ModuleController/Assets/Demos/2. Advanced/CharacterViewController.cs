using System;
using System.Collections;
using System.Collections.Generic;
using Sokka06.ModuleController;
using UnityEngine;

namespace Demos.Demo2
{
    public class CharacterViewController : ModuleControllerBehaviour<CharacterViewController, AbstractViewModule>
    {
        public CharacterModuleController Character;

        private void Awake()
        {
            SetupModules();
        }

        private void LateUpdate()
        {
            UpdateModules(Time.deltaTime);
        }
    }
}