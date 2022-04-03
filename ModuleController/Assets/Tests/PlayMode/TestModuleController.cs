using System;
using System.Collections;
using System.Collections.Generic;
using Sokka06.ModuleController;
using UnityEngine;

namespace Tests
{
    public class TestModuleController : ModuleControllerBehaviour<TestModuleController, AbstractTestModule>
    {
        protected override void Awake()
        {
            base.Awake();
            
            // Usually called on Start.
            SetupModules();
        }

        private void FixedUpdate()
        {
            UpdateModules(Time.deltaTime);
        }
    }
}