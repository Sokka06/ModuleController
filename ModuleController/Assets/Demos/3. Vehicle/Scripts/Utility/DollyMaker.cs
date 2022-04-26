using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

namespace Demos.Vehicle
{
    public class DollyMaker : MonoBehaviour
    {
        [Tooltip("Drop Waypoint Manager here to create a dolly track from waypoints.")]
        public PathManager pathManager;

        private void OnValidate()
        {
            if (pathManager != null)
            {
                var dollyTrack = GetComponent<CinemachineSmoothPath>();
                dollyTrack.m_Waypoints = CreateTrack(pathManager.Points.ToArray());
                pathManager = null;
            }
        }

        public CinemachineSmoothPath.Waypoint[] CreateTrack(PathPoint[] points)
        {
            var waypoints = new CinemachineSmoothPath.Waypoint[points.Length];
            for (int i = 0; i < waypoints.Length; i++)
            {
                waypoints[i] = PointToWaypoint(points[i].transform.position);
            }
            return waypoints;
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
