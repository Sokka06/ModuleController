using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public int Index = -1;
    
    public Vector3 Position3D { get; private set; }
    public Vector2 Position2D { get; private set; }

    public void Setup()
    {
        Position3D = transform.position;
        Position2D = new Vector2(Position3D.x, Position3D.z);
    }

    private void OnDrawGizmos()
    {
        var color = Color.green;
        color.a *= 0.5f;
        Gizmos.color = color;
        
        Gizmos.DrawSphere(transform.position, 0.5f);
    }
}
