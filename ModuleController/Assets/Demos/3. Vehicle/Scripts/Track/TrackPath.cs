using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

[Serializable] 
public struct TrackPoint
{
    public Vector3 Position;
    public float Width;
    public float Roll;

    public TrackPoint(Vector3 position = new Vector3(), float width = 1f, float roll = 0f)
    {
        Position = position;
        Width = width;
        Roll = roll;
    }

    /// <summary>Representation as Vector4</summary>
    internal Vector4 AsVector4 => new Vector4(Position.x, Position.y, Position.z, Roll);

    internal static TrackPoint FromVector4(Vector4 v)
    {
        var wp = new TrackPoint
        {
            Position = new Vector3(v[0], v[1], v[2]),
            Roll = v[3]
        };
        return wp;
    }
}
public class TrackPath : MonoBehaviour
{
    public TrackPoint[] m_Waypoints = Array.Empty<TrackPoint>();
}
