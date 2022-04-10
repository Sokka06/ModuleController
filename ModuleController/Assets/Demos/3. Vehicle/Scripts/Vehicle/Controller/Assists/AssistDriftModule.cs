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
    [Tooltip("How much to assist with spinning while drifting")]
    public float driftSpinAssist;
    public float driftSpinSpeed;
    public float driftSpinExponent = 1;

    [Tooltip("Automatically adjust drift angle based on steer input magnitude")]
    public bool autoSteerDrift;
    public float maxDriftAngle = 70;
    private float targetDriftAngle;
    
    [Tooltip("Adjusts the force based on drift speed, x-axis = speed, y-axis = force")]
    public AnimationCurve driftSpinCurve = AnimationCurve.Linear(0, 0, 10, 1);

    [Tooltip("How much to push the vehicle forward while drifting")]
    public float driftPush;

    [Tooltip("Straighten out the vehicle when sliding slightly")]
    public bool straightenAssist;
    
    /*[Header("Drift Module"), Tooltip("Amount of force added.")]
    public float Factor = 1f;

    public float SpeedMultiplier = 1.5f;
    public float Acceleration = 5f;
    [Range(0f, 1f), Tooltip("Drift push direction. 0f = forward, 1f = sideways")]
    public float Direction = 0.5f;*/

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
        var velocitySqrMag = Controller.Controller.Velocity.sqrMagnitude;
        
        if(!Enabled || !Controller.Controller.GroundData.IsGrounded || !(velocitySqrMag > 0f))
            return;

        var localVelocity = Controller.Controller.LocalVelocity;
        var localAngularVelocity =
            Controller.Controller.Transform.InverseTransformVector(Controller.Controller.AngularVelocity);

        // Get desired rotation speed
        var targetTurnSpeed = 0f;

        // Auto steer drift
        if (autoSteerDrift) {
            var steerSign = 0;
            if (_inputModule.Inputs.Steer != 0) {
                steerSign = (int)Mathf.Sign(_inputModule.Inputs.Steer);
            }

            targetDriftAngle = (steerSign != Mathf.Sign(localVelocity.x) ? _inputModule.Inputs.Steer : steerSign) * -maxDriftAngle;
            Vector3 velDir = new Vector3(localVelocity.x, 0, localVelocity.z).normalized;
            Vector3 targetDir = new Vector3(Mathf.Sin(targetDriftAngle * Mathf.Deg2Rad), 0, Mathf.Cos(targetDriftAngle * Mathf.Deg2Rad)).normalized;
            Vector3 driftTorqueTemp = velDir - targetDir;
            targetTurnSpeed = driftTorqueTemp.magnitude * Mathf.Sign(driftTorqueTemp.z) * steerSign * driftSpinSpeed - localAngularVelocity.y * Mathf.Clamp01(Vector3.Dot(velDir, targetDir)) * 2;
        }
        else {
            targetTurnSpeed = _inputModule.Inputs.Steer * driftSpinSpeed * (localVelocity.z < 0 ? (_driveModule.IsReversing ? Mathf.Sign(_inputModule.Inputs.Throttle) : Mathf.Sign(MaxAbs(_inputModule.Inputs.Throttle, -_inputModule.Inputs.Brake))) : 1);
        }

        Controller.Controller.Rigidbody.AddRelativeTorque(
            new Vector3(0, (targetTurnSpeed - localAngularVelocity.y) * driftSpinAssist * driftSpinCurve.Evaluate(Mathf.Abs(Mathf.Pow(localVelocity.x, driftSpinExponent))) * Controller.Controller.GroundData.Factor, 0),
            ForceMode.Acceleration);

        var rightVelDot = Vector3.Dot(Controller.Controller.Transform.right, Controller.Controller.Velocity.normalized);

        if (straightenAssist && _inputModule.Inputs.Steer == 0 && Mathf.Abs(rightVelDot) < 0.1f && velocitySqrMag > 5) {
            Controller.Controller.Rigidbody.AddRelativeTorque(
                new Vector3(0, rightVelDot * 100 * Mathf.Sign(localVelocity.z) * driftSpinAssist, 0),
                ForceMode.Acceleration);
        }
        
        /*if (!Enabled || !Controller.Controller.GroundData.IsGrounded || !(Controller.Controller.Rigidbody.velocity.sqrMagnitude > 0f))
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
        Controller.Controller.Rigidbody.AddForce(force, ForceMode.Acceleration);*/

        /*
        var forward = Vector3.Cross(Controller.Controller.GroundData.Normal, Controller.Controller.Transform.right);
        var right = Vector3.Cross(Controller.Controller.GroundData.Normal, Controller.Controller.Transform.forward);
        var sideVel = Vector3.Dot(Controller.Controller.Rigidbody.velocity, Controller.Controller.Transform.right);
        var dir = Vector3.Slerp(forward, right * Mathf.Sign(sideVel), Direction);
        
        Controller.Controller.Rigidbody.AddForce(dir * Controller.Controller.Rigidbody.velocity.magnitude * _inputModule.Inputs.Throttle * Factor, ForceMode.Acceleration);*/
    }
    
    public float MaxAbs(params float[] nums) {
        float result = 0;

        for (int i = 0; i < nums.Length; i++) {
            if (Mathf.Abs(nums[i]) > Mathf.Abs(result)) {
                result = nums[i];
            }
        }

        return result;
    }
}