using System;
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

    public float SpeedMultiplier = 1.5f;
    public float Acceleration = 5f;
    [Range(0f, 1f), Tooltip("Drift push direction. 0f = forward, 1f = sideways")]
    public float Direction = 0.5f;

    private VehicleInputModule _inputModule;
    private VehicleDriveModule _driveModule;

    public override void SetupModule(VehicleAssistsModule controller)
    {
        base.SetupModule(controller);

        _inputModule = Controller.Controller.GetModule<VehicleInputModule>();
        _driveModule = Controller.Controller.GetModule<VehicleDriveModule>();
    }

    public override void UpdateModule(float deltaTime)
    {
        if (!Enabled || !Controller.Controller.GroundData.IsGrounded || !(Controller.Controller.Rigidbody.velocity.sqrMagnitude > 0f))
            return;
        
        var currentVelocity = Controller.Controller.Rigidbody.velocity;
        var localVelocity = Controller.Controller.Transform.InverseTransformVector(currentVelocity);
            
        if (!(localVelocity.z > 0f))
            return;
            
        var right = Controller.Controller.Transform.right;
        var forward = Controller.Controller.Transform.forward;
            
        var rightDot = Vector3.Dot(currentVelocity.normalized, right);
        var driftDir = Vector3.Slerp(right * Math.Sign(rightDot),
            forward, Direction);

        var targetVelocity =
            driftDir * (_driveModule.Speed *
                        SpeedMultiplier); //turnVector * velocity.magnitude; //turnVector * AirMaxSpeed * boostMultiplier
        targetVelocity.y = currentVelocity.y;
            
        var forwardDot = Vector3.Dot(currentVelocity.normalized, forward);
            
        var localTargetVelocity = Controller.Controller.Transform.InverseTransformVector(targetVelocity);
            
        var velocityDiff = Controller.Controller.Transform.TransformVector(localTargetVelocity - localVelocity);
        var multiplier = (1f - Mathf.Abs(forwardDot)) * (Mathf.Abs(localVelocity.z) / _driveModule.Speed) *
                         _inputModule.Inputs.Throttle;
        var force = velocityDiff * (Acceleration * multiplier); // * (1f - Mathf.Abs(Vector3.Dot(forward, currentVelocity.normalized)))
        Controller.Controller.Rigidbody.AddForce(force, ForceMode.Acceleration);

        /*
        var forward = Vector3.Cross(Controller.Controller.GroundData.Normal, Controller.Controller.Transform.right);
        var right = Vector3.Cross(Controller.Controller.GroundData.Normal, Controller.Controller.Transform.forward);
        var sideVel = Vector3.Dot(Controller.Controller.Rigidbody.velocity, Controller.Controller.Transform.right);
        var dir = Vector3.Slerp(forward, right * Mathf.Sign(sideVel), Direction);
        
        Controller.Controller.Rigidbody.AddForce(dir * Controller.Controller.Rigidbody.velocity.magnitude * _inputModule.Inputs.Throttle * Factor, ForceMode.Acceleration);*/
    }
}