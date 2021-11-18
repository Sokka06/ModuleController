using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoomModule : AbstractCameraModule
{
    public override void SetupModule(CameraController controller)
    {
        base.SetupModule(controller);
        
        Debug.Log($"Hello {GetType().Name}");
    }

    public override void UpdateModule(float deltaTime)
    {
        
    }
}
