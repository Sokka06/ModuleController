using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Demos.Vehicle
{
    public struct TargetData
    {
        public Vector3 NearestPoint;
        public Vector3 LookAheadPoint;
        public float Distance;
    }
    
    public class RaceAI : MonoBehaviour
    {
        public float UpdateInterval;
        public float MaxCutDistance = 5f;
        public float LookAheadTime = 0.5f;
        
        private VehicleInputModule _inputModule;
        private VehicleSteerModule _steerModule;
        
        public Vehicle Vehicle { get; private set; }
        public WaypointManager WaypointManager { get; private set; }
        public Waypoint Target { get; private set; }
        public TargetData TargetData { get; private set; }

        private float _throttleScale;
        private float _currentTF;

        public void Setup(Vehicle vehicle)
        {
            AssignVehicle(vehicle);

            WaypointManager = FindObjectOfType<WaypointManager>();
            Target = WaypointManager.Waypoints[0];
        }

        private void Start()
        {
            _throttleScale = Random.Range(0.9f, 1f);
            _currentTF = WaypointManager.GetNearestPointTF(Vehicle.Controller.Transform.position);
        }

        public void AssignVehicle(Vehicle vehicle)
        {
            Vehicle = vehicle;
            if (Vehicle == null)
            {
                _inputModule = null;
                _steerModule = null;
                return;
            }
            
            _inputModule = Vehicle.Controller.GetModule<VehicleInputModule>();
            _steerModule = Vehicle.Controller.GetModule<VehicleSteerModule>();
        }

        private void FixedUpdate()
        {
            if (_inputModule == null)
                return;

            UpdateTarget(out var targetData);
            TargetData = targetData;
            
            var inputs = new VehicleInputs();
            inputs.Throttle = GetThrottle(Vehicle.transform.position, Target.transform.position);
            inputs.Steer = GetSteer(Vehicle.transform.position, Target.transform.position);
            inputs.Brake = GetBrake(Vehicle.transform.position, Target.transform.position);

            _inputModule.SetInputs(ref inputs);

            /*var inputs = new VehicleInputs();
            inputs.Throttle = 1f;
            inputs.Steer = Mathf.Sin(_sinTimer);

            _inputModule.SetInputs(ref inputs);

            var frequency = 1f;
            _sinTimer += Time.deltaTime * frequency;*/
        }

        private void UpdateTarget(out TargetData targetData)
        {
            var position = Vehicle.Controller.Transform.position;
            
            var nearestPointTF = WaypointManager.GetNearestPointTF(position);
            var lookAheadTF =
                WaypointManager.DistanceToTF(Vehicle.Controller.Rigidbody.velocity.magnitude * LookAheadTime);
            
            var nearestPoint = WaypointManager.Interpolate(nearestPointTF);
            var lookAheadPoint = WaypointManager.Interpolate(nearestPointTF + lookAheadTF);
            var distance = WaypointManager.TFToDistance(lookAheadTF);
            
            targetData = new TargetData
            {
                NearestPoint = nearestPoint,
                LookAheadPoint = lookAheadPoint,
                Distance = distance
            };

            var deltaTF = nearestPointTF - _currentTF;
            /*var position = Vehicle.transform.position;
            var targetPosition = Target.Position3D;
            var prevTargetPosition = WaypointManager.GetPrevious(Target.Index).Position3D;

            var reachedTarget = HasPassedWaypoint(position, prevTargetPosition, targetPosition);
            if (reachedTarget)
            {
                Target = WaypointManager.GetNext(Target);
            }*/

            /*var position = Vehicle.transform.position;
            var targetPosition = Target.transform.position;

            var reachedTarget = ReachedTarget(position, targetPosition, 10f);
            if (reachedTarget)
            {
                Target = WaypointManager.GetNext(Target);
            }*/
        }

        private bool ReachedTarget(Vector3 position, Vector3 targetPosition, float minDistance)
        {
            position.y = 0f;
            targetPosition.y = 0f;

            var sqrMag = (targetPosition - position).sqrMagnitude;

            return sqrMag < minDistance * minDistance;
        }

        private float GetSteer(Vector3 position, Vector3 target)
        {
            var steer = 0f;
            
            /*position.y = 0f;
            target.y = 0f;

            var velocity = Vehicle.Controller.Rigidbody.velocity;
            velocity.y = 0f;
            velocity.Normalize();
            
            var direction = (target - position).normalized;
            
            var right = Vector3.Cross(Vector3.up, velocity);

            right = Vehicle.transform.right;
            right.y = 0f;
            right.Normalize();

            var cte = GetCrossTrackError(position, WaypointManager.GetPrevious(Target.Index).Position3D,
                Target.Position3D);
            
            steer = Vector3.Dot(direction, right);*/

            var alpha = Vector3.SignedAngle(Vehicle.Controller.Transform.forward, (TargetData.LookAheadPoint - position).normalized, Vector3.up);
            var wheelBase = Vector3.Distance(Vehicle.Controller.Wheels[0].transform.localPosition, Vehicle.Controller.Wheels[2].transform.localPosition);

            var radius = wheelBase * 2f * Mathf.Sin(alpha * Mathf.Deg2Rad);
            var ld = Vector3.Distance(position, TargetData.LookAheadPoint);
            var steerAngle = Mathf.Atan(radius / ld) * Mathf.Rad2Deg;
            steer = steerAngle / _steerModule.SteerAngle;
            
            return steer;
        }
        
        private float GetThrottle(Vector3 position, Vector3 target)
        {
            var throttle = 1f;

            return throttle * 0.25f;
        }
        
        private float GetBrake(Vector3 position, Vector3 target)
        {
            var brake = 0f;
            
            position.y = 0f;
            target.y = 0f;

            var velocity = Vehicle.Controller.Rigidbody.velocity;
            velocity.y = 0f;
            velocity.Normalize();
            
            var direction = (target - position).normalized;
            
            var right = Vector3.Cross(Vector3.up, velocity);

            right = Vehicle.transform.right;
            right.y = 0f;
            right.Normalize();

            brake = Mathf.Abs(Vector3.Dot(direction, right));

            return 0f;
        }

        private void OnDrawGizmos()
        {
            /*var p1 = Vehicle.Controller.Transform.position;
            var closest = WaypointManager.GetNearestPoint(p1);
            var p2 = closest.Position3D;

            var next = WaypointManager.GetNext(closest.Index);
            var p3 = next.Position3D;
            var p4 = WaypointManager.GetNearestPointOnSegment(p1, p2, p3);

            var p5 = WaypointManager.GetClosestLine(p1);
            var p6 = WaypointManager.GetNearestPointOnSegment(p1, p5.from.Position3D, p5.to.Position3D);
            
            //Gizmos.DrawLine(p1, p2);
            Gizmos.DrawLine(p1, p6);*/
        }

        public float GetCrossTrackError(Vector3 position, Vector3 from, Vector3 to)
        {
            //The first part is the same as when we check if we have passed a waypoint
        
            //The vector between the character and the waypoint we are going from
            var a = position - from;

            //The vector between the waypoints
            var b = to - from;

            //Vector projection from https://en.wikipedia.org/wiki/Vector_projection
            //To know if we have passed the upcoming waypoint we need to find out how much of b is a1
            //a1 = (a.b / |b|^2) * b
            //a1 = progress * b -> progress = a1 / b -> progress = (a.b / |b|^2)
            var progress = (a.x * b.x + a.y * b.y + a.z * b.z) / (b.x * b.x + b.y * b.y + b.z * b.z);

            //The coordinate of the position where the car should be
            var errorPos = from + progress * b;

            //The error between the position where the car should be and where it is
            var error = (errorPos - position).magnitude;

            return error;
        }
        
        //From http://www.habrador.com/tutorials/linear-algebra/2-passed-waypoint/
        public static bool HasPassedWaypoint(Vector3 position, Vector3 from, Vector3 to)
        {
            var hasPassedWaypoint = false;

            //The vector between the character and the waypoint we are going from
            var a = position - from;

            //The vector between the waypoints
            var b = to - from;

            //Vector projection from https://en.wikipedia.org/wiki/Vector_projection
            //To know if we have passed the upcoming waypoint we need to find out how much of b is a1
            //a1 = (a.b / |b|^2) * b
            //a1 = progress * b -> progress = a1 / b -> progress = (a.b / |b|^2)
            var progress = (a.x * b.x + a.y * b.y + a.z * b.z) / (b.x * b.x + b.y * b.y + b.z * b.z);

            //If progress is above 1 we know we have passed the waypoint
            if (progress > 1.0f)
            {
                hasPassedWaypoint = true;
            }

            return hasPassedWaypoint;
        }
    }
}