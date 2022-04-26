using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace Demos.Vehicle
{
    public class DemoIntroModule : AbstractDemoModule
    {
        public override void SetupModule(RaceDemoController controller)
        {
            base.SetupModule(controller);
        }

        public override void EnterModule()
        {
            base.EnterModule();

            // Freeze all vehicles
            for (int i = 0; i < Controller.DriverManager.CurrentDrivers.Count; i++)
            {
                Controller.DriverManager.CurrentDrivers[i].Vehicle.SetFreeze(true);
            }
            
            // Move to ingame when any button is pressed.
            InputSystem.onAnyButtonPress.CallOnce(ctrl =>
            {
                Controller.SetState(Controller.GetModule<DemoIngameModule>());
            });
        }
    }
}

