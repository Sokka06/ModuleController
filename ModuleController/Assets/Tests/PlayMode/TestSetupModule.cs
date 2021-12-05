using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tests
{
    public class TestSetupModule : AbstractTestModule
    {
        public bool IsSetup { get; private set; }

        public override void SetupModule(TestModuleController controller)
        {
            base.SetupModule(controller);

            IsSetup = true;
        }
    }
}