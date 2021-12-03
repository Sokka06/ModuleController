using System.Collections;
using System.Collections.Generic;
using Demos.Vehicle;
using UnityEngine;

/// <summary>
/// Helps you drift by adding a sideways force.
/// </summary>
public class AssistDriftModule : AbstractAssistModule
{
    [Header("Drift Module"), Tooltip("Amount of force added.")]
    public float Factor = 1f;
    [Range(0f, 1f), Tooltip("Drift push direction. 0f = forward, 1f = sideways")]
    public float Direction = 0.5f;

    private VehicleInputModule _inputModule;

    public override void SetupModule(VehicleAssistsModule controller)
    {
        base.SetupModule(controller);

        _inputModule = Controller.Controller.GetModule<VehicleInputModule>();
    }

    public override void UpdateModule(float deltaTime)
    {
        if (!Enabled || !Controller.Controller.GroundData.IsGrounded || !(Controller.Controller.Rigidbody.velocity.sqrMagnitude > 0f))
            return;

        var forward = Vector3.Cross(Controller.Controller.GroundData.Normal, Controller.Controller.Transform.right);
        var right = Vector3.Cross(Controller.Controller.GroundData.Normal, Controller.Controller.Transform.forward);
        var sideVel = Vector3.Dot(Controller.Controller.Rigidbody.velocity, Controller.Controller.Transform.right);
        var dir = Vector3.Slerp(forward, right, Direction) * Mathf.Sign(sideVel);
        
        Controller.Controller.Rigidbody.AddForce(dir * Controller.Controller.Rigidbody.velocity.magnitude * _inputModule.Inputs.Throttle * Factor, ForceMode.Acceleration);
    }
}