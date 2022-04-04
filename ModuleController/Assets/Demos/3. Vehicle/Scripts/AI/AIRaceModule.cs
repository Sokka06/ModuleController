using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demos.Vehicle
{
    public class AIRaceModule : AbstractAIModule
    {
        public float MaxCutDistance = 5f;
        public float LookAheadTime = 0.5f;
        
        private VehicleInputModule _inputModule;
        private VehicleSteerModule _steerModule;
        
        private float _throttleScale;
        private float _currentTF;        
        
        public TargetData TargetData { get; private set; }
        public TargetData PrevTargetData { get; private set; }

        public override void SetupModule(DriverAIController controller)
        {
            base.SetupModule(controller);
            
            _throttleScale = Random.Range(0.9f, 1f);
            _currentTF = Controller.WaypointManager.GetNearestPointTF(Controller.Vehicle.Controller.Transform.position);
        }

        public override void AssignVehicle(Vehicle vehicle)
        {
            base.AssignVehicle(vehicle);
            
            if (vehicle == null)
            {
                _inputModule = null;
                _steerModule = null;
                return;
            }
            
            _inputModule = vehicle.Controller.GetModule<VehicleInputModule>();
            _steerModule = vehicle.Controller.GetModule<VehicleSteerModule>();
        }

        public override void UpdateState(float deltaTime)
        {
            base.UpdateState(deltaTime);
            
            
        }

        public override void UpdateModule(float deltaTime)
        {
            base.UpdateModule(deltaTime);
            
            if  (!Enabled || Controller.State != AIState.Race || _inputModule == null)
                return;

            UpdateTarget(out var targetData);
            PrevTargetData = TargetData;
            TargetData = targetData;
            
            var inputs = new VehicleInputs();
            inputs.Throttle = GetThrottle(Controller.Vehicle.transform.position);
            inputs.Steer = GetSteer(Controller.Vehicle.transform.position);
            inputs.Brake = GetBrake(Controller.Vehicle.transform.position);

            _inputModule.SetInputs(ref inputs);
        }
        
        private void UpdateTarget(out TargetData targetData)
        {
            var position = Controller.Vehicle.Controller.Transform.position;
            
            var nearestPointTF = Controller.WaypointManager.GetNearestPointTF(position);
            var lookAheadTF =
                Controller.WaypointManager.DistanceToTF(Mathf.Max(Controller.Vehicle.Controller.Rigidbody.velocity.magnitude * LookAheadTime, 1f));
            
            var deltaTF = nearestPointTF - _currentTF;
            
            if (Mathf.Sign(deltaTF) < 0)
            {
                Debug.Log("<color=red>CANT GO BACK</color>");
            }
            var deltaDistance = Controller.WaypointManager.TFToDistance(deltaTF);
            if (deltaDistance > MaxCutDistance)
            {
                Debug.Log("<color=blue>MASSIVE CUT</color>");
            }
            
            _currentTF = nearestPointTF;
            
            var nearestPoint = Controller.WaypointManager.Interpolate(_currentTF, out var nearestForward);
            var lookAheadPoint = Controller.WaypointManager.Interpolate(_currentTF + lookAheadTF, out var lookAheadForward);
            var distance = Controller.WaypointManager.TFToDistance(lookAheadTF);
            
            targetData = new TargetData
            {
                NearestPoint = nearestPoint,
                NearestForward = nearestForward,
                LookAheadPoint = lookAheadPoint,
                LookAheadForward = lookAheadForward,
                Distance = distance
            };
        }

        private float GetSteer(Vector3 position)
        {
            var steer = 0f;

            var alpha = Vector3.SignedAngle(Controller.Vehicle.Controller.Transform.forward, (TargetData.LookAheadPoint - position).normalized, Vector3.up);
            var wheelBase = Vector3.Distance(Controller.Vehicle.Controller.Wheels[0].transform.localPosition, Controller.Vehicle.Controller.Wheels[2].transform.localPosition);

            var radius = wheelBase * 2f * Mathf.Sin(alpha * Mathf.Deg2Rad);
            var ld = Vector3.Distance(position, TargetData.LookAheadPoint);
            var steerAngle = Mathf.Atan(radius / ld) * Mathf.Rad2Deg;
            steer = steerAngle / _steerModule.SteerAngle;
            
            return steer;
        }

        private float StanleySteer()
        {
            var segment = Controller.WaypointManager.TFToSegment(_currentTF);

            var yaw = 0f;
            var x = 0f;
            var y = 0f;
            var k_e = 0f;
            var k_v = 0f;
            var v = 0f;
            
            var last_point_on_trajectory = segment.Start.Position3D;
            var first_point_on_trajectory = segment.End.Position3D;
            var yaw_path = Mathf.Atan2(last_point_on_trajectory.z - first_point_on_trajectory.z,
                last_point_on_trajectory.x - first_point_on_trajectory.x) * Mathf.Rad2Deg;
            var yaw_diff = yaw_path - yaw;

            yaw_diff = NormalizeAngle(yaw_diff);

            var center_axle_current = Vector3.Lerp(Controller.Vehicle.Controller.Wheels[0].transform.position, Controller.Vehicle.Controller.Wheels[1].transform.position, 0.5f);
            var crosstrack_error = 0f;//np.min(np.sum((center_axle_current - np.array(waypoints)[:, :2]) ** 2, axis=1));
            var yaw_cross_track = Mathf.Atan2(y - first_point_on_trajectory.y, x - first_point_on_trajectory.x);
            var yaw_diff_of_path_cross_track = NormalizeAngle(yaw_path - yaw_cross_track);
            crosstrack_error = yaw_diff_of_path_cross_track > 0
                ? Mathf.Abs(crosstrack_error)
                : -Mathf.Abs(crosstrack_error);

            var yaw_diff_crosstrack = Mathf.Atan(k_e * crosstrack_error / (k_v + v));

            var expected_steering_angle = Mathf.Clamp(NormalizeAngle(yaw_diff + yaw_diff_crosstrack), -1.22f, 1.22f);
            
            return 0f;
        }

        private float NormalizeAngle(float angle)
        {
            return angle;
        }

        private float PurePursuitSteer(Vector3 position)
        {
            var alpha = Vector3.SignedAngle(Controller.Vehicle.Controller.Transform.forward, (TargetData.LookAheadPoint - position).normalized, Vector3.up);
            var wheelBase = Vector3.Distance(Controller.Vehicle.Controller.Wheels[0].transform.localPosition, Controller.Vehicle.Controller.Wheels[2].transform.localPosition);

            var radius = wheelBase * 2f * Mathf.Sin(alpha * Mathf.Deg2Rad);
            var ld = Vector3.Distance(position, TargetData.LookAheadPoint);
            var steerAngle = Mathf.Atan(radius / ld) * Mathf.Rad2Deg;
            return steerAngle / _steerModule.SteerAngle;
        }
        
        private float GetThrottle(Vector3 position)
        {
            var throttle = 1f;

            var dot = Vector3.Dot(Controller.Vehicle.Controller.Rigidbody.velocity.normalized, TargetData.LookAheadForward);
            return dot * 0.5f;
        }
        
        private float GetBrake(Vector3 position)
        {
            var brake = 0f;

            var dot = Vector3.Dot(Controller.Vehicle.Controller.Rigidbody.velocity.normalized, TargetData.LookAheadForward);
            
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

            brake = Mathf.Abs(Vector3.Dot(direction, right));
            */

            return 0f;
        }

        private void OnDrawGizmos()
        {
            if (!Application.isPlaying)
                return;
            
            Gizmos.DrawLine(Controller.Vehicle.Controller.Transform.position, TargetData.LookAheadPoint);
        }

        /*public float GetCrossTrackError(Vector3 position, Vector3 from, Vector3 to)
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
        }*/
    }
}

