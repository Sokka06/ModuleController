using System;
using System.Collections;
using System.Collections.Generic;
using Sokka06.ModuleController;
using UnityEngine;

public class CameraController : ModuleControllerBehaviour<CameraController, AbstractCameraModule>
{
    private void Awake()
    {
        SetupModules();
    }

    private void LateUpdate()
    {
        UpdateModules(Time.deltaTime);
    }
}
