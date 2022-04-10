using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demos.Vehicle
{
    public struct Segment
    {
        public PathPoint Start;
        public PathPoint End;
        
        public float Length;
        public float TF;
        
        public Vector3 Forward;
        public Vector3 Normal;
        public Vector3 Tangent;
    }

    public class WaypointManager : MonoBehaviour
    {
        public List<PathPoint> Points;
        public string NamePrefix = "PathPoint";

        public List<Segment> Segments { get; private set; }
        private float _totalLength;

        private void OnValidate()
        {
            UpdatePoint();
        }

        private void Awake()
        {
            for (int i = 0; i < Points.Count; i++)
            {
                Points[i].Setup();
            }
            
            // Calculate total length
            var totalLength = 0f;
            for (int i = 0; i < Points.Count; i++)
            {
                var start = Points[i];
                var end = GetNextPoint(start.Index);
                totalLength += Vector3.Distance(start.Position3D, end.Position3D);
            }
            _totalLength = totalLength;
            
            // Make segments.
            Segments = new List<Segment>(Points.Count);
            for (int i = 0; i < Points.Count; i++)
            {
                var start = Points[i];
                var end = GetNextPoint(start.Index);
                var length = Vector3.Distance(start.Position3D, end.Position3D);
                var tf = length / _totalLength;
                
                var forward = (end.Position3D - start.Position3D).normalized;
                var tangent = -Vector3.Cross(forward, Vector3.up);
                var normal = Vector3.Cross(forward, tangent);
                
                Segments.Add(new Segment
                {
                    Start = start,
                    End = end,
                    Length = length,
                    TF = tf,
                    Forward = forward,
                    Normal = normal,
                    Tangent = tangent
                });
            }

            /*var addedTF = 0f;
            for (int i = 0; i < Segments.Count; i++)
            {
                addedTF += Segments[i].TF;
            }

            Debug.Log($"Waypoints: {Waypoints.Count}, Segments: {Segments.Count}, Length: {_totalLength}, TF: {addedTF}");*/
        }

        public void SetupPoints()
        {
            for (int i = 0; i < Points.Count; i++)
            {
                Points[i].Index = i;
            }
        }
        
        public void FindPoints()
        {
            Points = new List<PathPoint>(transform.GetComponentsInChildren<PathPoint>());
        }
        
        public void RenamePoints()
        {
            for (int i = 0; i < Points.Count; i++)
            {
                Points[i].name = $"{NamePrefix} {i}";
            }
        }

        public PathPoint GetNextPoint(int index)
        {
            /*var nextIndex = index + 1;
            if (nextIndex >= Waypoints.Count)
                nextIndex = 0;*/
            
            var nextIndex = nfmod((index + 1), Points.Count);
            return Points[nextIndex];
        }
        
        public PathPoint GetNextPoint(PathPoint point)
        {
            var currentIndex = point.Index == -1 ? Points.IndexOf(point) : point.Index;
            var nextIndex = currentIndex + 1;
            if (nextIndex >= Points.Count)
                nextIndex = 0;
            
            return Points[nextIndex];
        }
        
        /// <summary>
        /// Gets previous waypoint from given index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public PathPoint GetPreviousPoint(int index)
        {
            /*var prevIndex = index - 1;
            if (prevIndex < 0)
                prevIndex = Waypoints.Count - 1;
                */

            var prevIndex = nfmod((index - 1), Points.Count);
            return Points[prevIndex];
        }
        
        /// <summary>
        /// Modulo that works with negative values.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private int nfmod(int a,int b)
        {
            return (a % b + b) % b;
        }

        /// <summary>
        /// Finds nearest Waypoint from given point.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public PathPoint GetNearestPathPoint(Vector3 point)
        {
            var closestWaypoint = default(PathPoint);
            var closestDistance = float.MaxValue;

            for (int i = 0; i < Points.Count; i++)
            {
                var waypoint = Points[i];
                var distance = Math.Abs(point.x - waypoint.Position3D.x) + Math.Abs(point.y - waypoint.Position3D.y) +
                               Math.Abs(point.z - waypoint.Position3D.z);
                if(distance > closestDistance)
                    continue;

                closestWaypoint = waypoint;
                closestDistance = distance;
            }
            
            return closestWaypoint;
        }

        /// <summary>
        /// Finds nearest segment from given point.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public Segment GetNearestSegment(Vector3 point)
        {
            var nearestSegment = default(Segment);
            var nearestDistance = float.MaxValue;

            for (int i = 0; i < Segments.Count; i++)
            {
                var segment = Segments[i];
                var nearestPoint = GetNearestPointOnSegment(point, segment.Start.Position3D, segment.End.Position3D);
                var distance = Math.Abs(nearestPoint.x - point.x) + Math.Abs(nearestPoint.y - point.y) +
                               Math.Abs(nearestPoint.z - point.z);
                
                if(distance > nearestDistance)
                    continue;

                nearestSegment = segment;
                nearestDistance = distance;
            }
            
            return nearestSegment;
        }

        /// <summary>
        /// Finds nearest TF from given point.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public float GetNearestPointTF(Vector3 point)
        {
            var nearestSegment = GetNearestSegment(point);
            var segmentIndex = Segments.IndexOf(nearestSegment);
            var currentTF = 0f;
            for (int i = 0; i < segmentIndex; i++)
            {
                var segment = Segments[i];
                currentTF += segment.TF;
            }
            
            var nearestPoint = GetNearestPointOnSegment(point, nearestSegment.Start.Position3D, nearestSegment.End.Position3D);
            var distance = Vector3.Distance(nearestSegment.Start.Position3D, nearestPoint);
            var distanceTF = DistanceToTF(distance);
            
            return currentTF + distanceTF;
        }

        /// <summary>
        /// Finds nearest point on line from given point.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public Vector3 GetNearestPoint(Vector3 point)
        {
            var nearestSegment = GetNearestSegment(point);
            return GetNearestPointOnSegment(point, nearestSegment.Start.Position3D, nearestSegment.End.Position3D);
        }

        /// <summary>
        /// Finds nearest point on segment.
        /// </summary>
        /// <param name="point"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public Vector3 GetNearestPointOnSegment(Vector3 point, Vector3 from, Vector3 to)
        {
            var line = (to - from);
            var len = line.magnitude;
            line.Normalize();
       
            var v = point - from;
            var d = Vector3.Dot(v, line);
            d = Mathf.Clamp(d, 0f, len);
            return from + line * d;
        }

        /// <summary>
        /// Finds point from given TF.
        /// </summary>
        /// <param name="tf"></param>
        /// <returns></returns>
        public Vector3 Interpolate(float tf, out Vector3 forward)
        {
            if (tf > 1f)
                tf %= 1f;
            
            var currentTF = 0f;

            for (int i = 0; i < Segments.Count; i++)
            {
                var segment = Segments[i];

                currentTF += segment.TF;
                if (currentTF < tf)
                    continue;
                
                // Reached the segment
                forward = segment.Forward;
                
                var startTF = currentTF - segment.TF;
                var t = Mathf.InverseLerp(startTF, currentTF, tf);
                return Vector3.Lerp(segment.Start.Position3D, segment.End.Position3D, t);
            }

            forward = Vector3.zero;
            return Vector3.zero;
        }

        /// <summary>
        /// Converts TF to world space distance.
        /// </summary>
        /// <param name="tf"></param>
        /// <returns></returns>
        public float TFToDistance(float tf)
        {
            return _totalLength * tf;
        }
        
        /// <summary>
        /// Converts world space distance to TF.
        /// </summary>
        /// <param name="distance"></param>
        /// <returns></returns>
        public float DistanceToTF(float distance)
        {
            return distance / _totalLength;
        }

        /// <summary>
        /// Finds segment from given TF.
        /// </summary>
        /// <param name="tf"></param>
        /// <param name="index">-1 if no segment found.</param>
        /// <returns></returns>
        public Segment TFToSegment(float tf, out int index)
        {
            index = -1;
            
            // Wrap TF to 0f-1f
            if (tf > 1f)
                tf %= 1f;
            
            var currentTF = 0f;

            for (int i = 0; i < Segments.Count; i++)
            {
                var segment = Segments[i];

                currentTF += segment.TF;
                if (currentTF < tf)
                    continue;
                
                // Found the segment
                index = i;
                return segment;
            }
            
            return default;
        }

        public float SegmentToTF(Segment segment)
        {
            return 0f;
        }

        /*/// <summary>
        /// Calculates total difference in angle between two TFs.
        /// </summary>
        /// <param name="fromTF"></param>
        /// <param name="toTF"></param>
        /// <returns></returns>
        public float GetAngleDifference(float fromTF, float toTF)
        {
            var error = 0f;

            TFToSegment(fromTF, out var fromIndex);
            TFToSegment(toTF, out var toIndex);

            var currentForward = Vector3.zero;
            var nextForward = Vector3.zero;

            var index = fromIndex;
            var iterations = 0;

            while (iterations < Segments.Count)
            {
                iterations++;
                
                var nextIndex = nfmod(index + 1, Segments.Count);
                currentForward = Segments[index].Forward;
                nextForward = Segments[nextIndex].Forward;
                
                error += Vector3.SignedAngle(currentForward, nextForward, Vector3.up);
                index = nextIndex;
                
                if (index == toIndex)
                    break;
            }

            return error;
        }*/

        [ContextMenu("Update Waypoints")]
        public void UpdatePoint()
        {
            FindPoints();
            SetupPoints();
            RenamePoints();
        }

        private void OnDrawGizmos()
        {
            var color = Color.green;
            color.a *= 0.5f;
            Gizmos.color = color;
            
            for (int i = 0; i < Points.Count; i++)
            {
                var current = Points[i];
                var next = GetNextPoint(i);
                if (current == null || next == null)
                    continue;
                
                Gizmos.DrawLine(current.transform.position, next.transform.position);
            }
        }
    }
}