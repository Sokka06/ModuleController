using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public struct PlayerCameraInputs
{
    public Vector2 Axis;
}

/// <summary>
/// Provides input to free look camera
/// </summary>
public class PlayerInputProvider : MonoBehaviour, AxisState.IInputAxisProvider
{
    public PlayerCameraInputs Inputs { get; private set; }
    
    public void SetInputs(ref PlayerCameraInputs inputs)
    {
        Inputs = inputs;
    }
    
    /// <summary>
    /// Implementation of AxisState.IInputAxisProvider.GetAxisValue().
    /// Axis index ranges from 0...2 for X, Y, and Z.
    /// Reads the action associated with the axis.
    /// </summary>
    /// <param name="axis"></param>
    /// <returns>The current axis value</returns>
    public virtual float GetAxisValue(int axis)
    {
        switch (axis)
        {
            case 0: return Inputs.Axis.x;
            case 1: return Inputs.Axis.y;
        }
        return 0;
    }
}