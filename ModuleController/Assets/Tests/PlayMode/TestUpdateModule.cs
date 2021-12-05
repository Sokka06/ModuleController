using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tests
{
    public class TestUpdateModule : AbstractTestModule
    {
        public float RunTime { get; private set; }

        public override void SetupModule(TestModuleController controller)
        {
            base.SetupModule(controller);
        
        }

        public override void UpdateModule(float deltaTime)
        {
            base.UpdateModule(deltaTime);

            RunTime += deltaTime;
        }
    }
}