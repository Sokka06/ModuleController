using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct PIDValues
{
    public float Kp;
    public float Ki;
    public float Kd;

    public PIDValues(float kp, float ki, float kd)
    {
        Kp = kp;
        Ki = ki;
        Kd = kd;
    }
}

/// <summary>
/// A basic PID Controller. https://en.wikipedia.org/wiki/PID_controller
/// </summary>
public class PIDController
{
    private PIDValues _values;
    
    private float _integral;
    private float _integralMax;
    private float _prevError;
    
    private const float MaxOutput = 1000.0f;
    
    public PIDValues Values
    {
        get => _values;
        set
        {
            _values = value;
            
            _integralMax = MaxOutput / _values.Ki;
            _integral = Mathf.Clamp(_integral, -_integralMax, _integralMax);
        }
    }
    
    public PIDController(PIDValues values)
    {
        Values = values;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="current"></param>
    /// <param name="target"></param>
    /// <param name="deltaTime"></param>
    /// <returns></returns>
    public float GetOutput(float current, float target, float deltaTime)
    {
        var error = target - current;
        _integral = Mathf.Clamp(_integral + error * deltaTime, -_integralMax, _integralMax);

        var derivative = (error - _prevError) / deltaTime;
        var output = Values.Kp * error + Values.Ki * _integral + Values.Kd * derivative;
        
        _prevError = error;
        return Mathf.Clamp(output, -MaxOutput, MaxOutput);
    }
}