using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Simply reads mouse input and gives it to free look camera.
/// </summary>
public class SimpleInputProvider : MonoBehaviour, AxisState.IInputAxisProvider
{
    /// <summary>
    /// Implementation of AxisState.IInputAxisProvider.GetAxisValue().
    /// Axis index ranges from 0...2 for X, Y, and Z.
    /// Reads the action associated with the axis.
    /// </summary>
    /// <param name="axis"></param>
    /// <returns>The current axis value</returns>
    public virtual float GetAxisValue(int axis)
    {
        var mouse = Mouse.current;
        if (mouse == null)
            return 0;
        
        switch (axis)
        {
            case 0: return mouse.delta.ReadValue().x;
            case 1: return mouse.delta.ReadValue().y;
        }
        return 0;
    }
}
