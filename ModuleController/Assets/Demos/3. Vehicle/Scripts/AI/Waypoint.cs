using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public int Index = -1;
    
    private void OnDrawGizmos()
    {
        var color = Color.green;
        color.a *= 0.5f;
        Gizmos.color = color;
        
        Gizmos.DrawSphere(transform.position, 0.5f);
    }
}
