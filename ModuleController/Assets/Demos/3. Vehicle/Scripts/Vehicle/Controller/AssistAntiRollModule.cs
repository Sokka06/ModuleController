using System.Collections;
using System.Collections.Generic;
using Demos.Vehicle;
using UnityEngine;

public class AssistAntiRollModule : AbstractAssistModule
{
    [Range(0f, 1f)]
    public float Factor = 1f;

    private Vector3 _initialCenterOfMass;
    private bool _prevIsGrounded;

    public override void SetupModule(VehicleAssistsModule controller)
    {
        base.SetupModule(controller);

        _initialCenterOfMass = Controller.Controller.Rigidbody.centerOfMass;
        Debug.Log("SETUP");
    }

    public override void UpdateModule(float deltaTime)
    {
        base.UpdateModule(deltaTime);

        if (Controller.Controller.GroundData.IsGrounded)
        {
            var centerOfMass = _initialCenterOfMass;
            centerOfMass.y = Mathf.Lerp(_initialCenterOfMass.y, 0f, Factor);
            Controller.Controller.Rigidbody.centerOfMass = centerOfMass;
        }

        // Left ground, reset center of mass.
        if (_prevIsGrounded && !Controller.Controller.GroundData.IsGrounded)
        {
            Controller.Controller.Rigidbody.centerOfMass = _initialCenterOfMass;
        }

        _prevIsGrounded = Controller.Controller.GroundData.IsGrounded;
    }
}
