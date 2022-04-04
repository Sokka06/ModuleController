using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

namespace Demos.Vehicle
{
    public class DemoFinishModule : AbstractDemoModule
    {
        public DriverManager DriverManager;
        public CinemachineVirtualCameraBase FinishCamera;
        
        public override void EnterModule()
        {
            base.EnterModule();
            
            // Assuming first racer is the player.
            var target = DriverManager.CurrentDrivers[0].Vehicle.transform;
            FinishCamera.Follow = target;
            FinishCamera.LookAt = target;
        }
    }
}