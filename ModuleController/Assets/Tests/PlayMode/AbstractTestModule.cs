using System.Collections;
using System.Collections.Generic;
using Sokka06.ModuleController;
using UnityEngine;

namespace Tests
{
    public abstract class AbstractTestModule : ModuleBehaviour<TestModuleController, AbstractTestModule>
    {
        public override void UpdateModule(float deltaTime)
        {

        }
    }
}