using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointManager : MonoBehaviour
{
    public List<Waypoint> Waypoints;
    public string NamePrefix = "Waypoint";

    private void OnValidate()
    {
        UpdateWaypoints();
    }

    public void SetupWaypoints()
    {
        for (int i = 0; i < Waypoints.Count; i++)
        {
            Waypoints[i].Index = i;
        }
    }
    
    public void FindWaypoints()
    {
        Waypoints = new List<Waypoint>(transform.GetComponentsInChildren<Waypoint>());
    }
    
    public void RenameWaypoints()
    {
        for (int i = 0; i < Waypoints.Count; i++)
        {
            Waypoints[i].name = $"{NamePrefix} {i}";
        }
    }

    public Waypoint GetNext(int index)
    {
        var nextIndex = index + 1;
        if (nextIndex >= Waypoints.Count)
            nextIndex = 0;
        
        return Waypoints[nextIndex];
    }
    
    public Waypoint GetNext(Waypoint waypoint)
    {
        var currentIndex = waypoint.Index == -1 ? Waypoints.IndexOf(waypoint) : waypoint.Index;
        var nextIndex = currentIndex + 1;
        if (nextIndex >= Waypoints.Count)
            nextIndex = 0;
        
        return Waypoints[nextIndex];
    }

    [ContextMenu("Update Waypoints")]
    public void UpdateWaypoints()
    {
        FindWaypoints();
        SetupWaypoints();
        RenameWaypoints();
    }

    private void OnDrawGizmos()
    {
        var color = Color.green;
        color.a *= 0.5f;
        Gizmos.color = color;
        
        for (int i = 0; i < Waypoints.Count; i++)
        {
            var current = Waypoints[i];
            var next = GetNext(i);
            if (current == null || next == null)
                continue;
            
            Gizmos.DrawLine(current.transform.position, next.transform.position);
        }
    }
}
