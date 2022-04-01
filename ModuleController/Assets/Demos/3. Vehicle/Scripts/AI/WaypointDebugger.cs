using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WaypointDebugger : MonoBehaviour
{
    public WaypointManager WaypointManager;

    [Space] 
    public float Size = 0.25f;
    
    
    private void OnDrawGizmos()
    {
        var pos = transform.position;
        
        var color = Color.yellow;
        color.a *= 0.5f;
        Gizmos.color = color;
        Gizmos.DrawSphere(pos, Size);
        
        if (!Application.isPlaying || WaypointManager == null)
            return;

        var nearestSegment = WaypointManager.GetNearestSegment(pos);
        var nearestPoint =
            WaypointManager.GetNearestPointOnSegment(pos, nearestSegment.Start.Position3D,
                nearestSegment.End.Position3D);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(pos, nearestPoint);

        var nearestTF = WaypointManager.GetNearestPointTF(pos);
        var interpolatePoint = WaypointManager.Interpolate(nearestTF, out var interpolateForward);
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(pos, interpolatePoint);

        var lookaheadTF = WaypointManager.DistanceToTF(5f);
        var lookaheadPoint = WaypointManager.Interpolate(nearestTF + lookaheadTF, out var lookAheadForward);
        Gizmos.DrawLine(pos, lookaheadPoint);
        
        // Test TF to distanc
        /*var tfDistance = WaypointManager.DistanceToTF(1f);
        var tfPoint = WaypointManager.Interpolate(tfDistance);
        Gizmos.DrawSphere(tfPoint, 0.1f);*/
        
        // Segment debug
        for (int i = 0; i < WaypointManager.Segments.Count; i++)
        {
            var segment = WaypointManager.Segments[i];
            var center = Vector3.Lerp(segment.Start.Position3D, segment.End.Position3D, 0.5f);
            
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(center, segment.Forward);
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(center, segment.Normal);
            Gizmos.color = Color.red;
            Gizmos.DrawRay(center, segment.Tangent);

            Handles.Label(center, $"Segment {i}");
        }
    }
}
