using System;
using System.Collections;
using System.Collections.Generic;
using Demos.Vehicle;
using UnityEngine;

namespace Demos.Vehicle
{
    /// <summary>
/// Helps you drift by adding a sideways force.
/// </summary>
public class AssistDriftModule : AbstractAssistModule
{
    [Header("Drift Assist")]
    [Tooltip("Automatically adjust drift angle based on steer input magnitude")]
    public bool AutoSteerDrift = true;
    [Tooltip("Straighten out the vehicle when sliding slightly")]
    public bool StraightenAssist = true;
    
    [Space, Tooltip("Adjusts the force based on drift speed, x-axis = speed, y-axis = force")]
    public AnimationCurve SpinCurve = AnimationCurve.Linear(0, 0, 10, 1);
    public float MaxDriftAngle = 70;
    
    [Space, Tooltip("How much to assist with spinning while drifting")]
    public float SpinAssist = 2;
    public float SpinSpeed = 3;
    public float SpinExponent = 2;

    [Space, Tooltip("How much to push the vehicle forward while drifting")]
    public float DriftPush = 3;

    

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

        // Snippet from https://github.com/JustInvoke/Randomation-Vehicle-Physics/blob/master/Assets/Scripts/Vehicle%20Control/VehicleAssist.cs
        
        // Get desired rotation speed
        var targetTurnSpeed = 0f;
        var targetDriftAngle = 0f;

        // Auto steer drift
        if (AutoSteerDrift) 
        {
            var steerSign = 0;
            if (_inputModule.Inputs.Steer != 0) 
                steerSign = (int)Mathf.Sign(_inputModule.Inputs.Steer);

            targetDriftAngle = (steerSign != (int)Mathf.Sign(localVelocity.x) ? _inputModule.Inputs.Steer : steerSign) * -MaxDriftAngle;
            
            var velDir = new Vector3(localVelocity.x, 0, localVelocity.z).normalized;
            var targetDir = new Vector3(Mathf.Sin(targetDriftAngle * Mathf.Deg2Rad), 0, Mathf.Cos(targetDriftAngle * Mathf.Deg2Rad)).normalized;
            var driftTorqueTemp = velDir - targetDir;
            targetTurnSpeed = driftTorqueTemp.magnitude * Mathf.Sign(driftTorqueTemp.z) * steerSign * SpinSpeed - localAngularVelocity.y * Mathf.Clamp01(Vector3.Dot(velDir, targetDir)) * 2;
        }
        else 
        {
            targetTurnSpeed = _inputModule.Inputs.Steer * SpinSpeed * 
                              (localVelocity.z < 0 ? _driveModule.IsReversing ? Mathf.Sign(_inputModule.Inputs.Throttle) : 
                                  Mathf.Sign(MaxAbs(_inputModule.Inputs.Throttle, -_inputModule.Inputs.Brake)) : 1);
        }

        Controller.Controller.Rigidbody.AddRelativeTorque(
            new Vector3(0, (targetTurnSpeed - localAngularVelocity.y) * SpinAssist * 
                           SpinCurve.Evaluate(Mathf.Abs(Mathf.Pow(localVelocity.x, SpinExponent))) * Controller.Controller.GroundData.Factor, 0), ForceMode.Acceleration);

        var rightVelDot = Vector3.Dot(Controller.Controller.Transform.right, Controller.Controller.Velocity.normalized);

        if (StraightenAssist && Mathf.Abs(_inputModule.Inputs.Steer) < 0.1f && Mathf.Abs(rightVelDot) < 0.1f && velocitySqrMag > 5) 
        {
            Controller.Controller.Rigidbody.AddRelativeTorque(
                new Vector3(0, rightVelDot * 100 * Mathf.Sign(localVelocity.z) * SpinAssist, 0),
                ForceMode.Acceleration);
        }

        /*if (driftPush > 0f)
        {
            var pushFactor = (_driveModule.IsReversing ? _inputModule.Inputs.Throttle : _inputModule.Inputs.Throttle - _inputModule.Inputs.Brake) * Mathf.Abs(localVelocity.x) * driftPush * Controller.Controller.GroundData.Factor * (1 - Mathf.Abs(Vector3.Dot(Controller.Controller.Transform.forward, Controller.Controller.Velocity.normalized)));

            Controller.Controller.AddVelocity(
                Controller.Controller.Transform.TransformDirection(new Vector3(Mathf.Abs(pushFactor) * Mathf.Sign(localVelocity.x), Mathf.Abs(pushFactor) * Mathf.Sign(localVelocity.z), 0)),
                ForceMode.Acceleration);
        }*/
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
}
