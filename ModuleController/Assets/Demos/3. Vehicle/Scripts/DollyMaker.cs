using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

namespace Demos.Vehicle
{
    public class DollyMaker : MonoBehaviour
    {
        public WaypointManager WaypointManager;

        private void OnValidate()
        {
            if (WaypointManager != null)
            {
                var dollyTrack = GetComponent<CinemachineSmoothPath>();
                var waypoints = new CinemachineSmoothPath.Waypoint[WaypointManager.Points.Count];
                for (int i = 0; i < waypoints.Length; i++)
                {
                    waypoints[i] = PointToWaypoint(WaypointManager.Points[i].transform.position);
                }
                dollyTrack.m_Waypoints = waypoints;

                WaypointManager = null;
            }
        }

        private CinemachineSmoothPath.Waypoint PointToWaypoint(Vector3 point)
        {
            return new CinemachineSmoothPath.Waypoint
            {
                position = point
            };
        }
    }
}
