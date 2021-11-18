using System.Collections;
using System.Collections.Generic;
using Sokka06.ModuleController;
using UnityEngine;

public abstract class AbstractCameraModule : ModuleBehaviour<CameraController, AbstractCameraModule>
{
    public override void SetupModule(CameraController controller)
    {
        base.SetupModule(controller);
    }
}
