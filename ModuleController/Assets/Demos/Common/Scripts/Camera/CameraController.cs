using System;
using System.Collections;
using System.Collections.Generic;
using Sokka06.ModuleController;
using UnityEngine;

namespace Demos.Common
{
    public class CameraController : ModuleControllerBehaviour<CameraController, AbstractCameraModule>
    {
        private void Start()
        {
            SetupModules();
        }

        private void LateUpdate()
        {
            UpdateModules(Time.deltaTime);
        }
    }
}