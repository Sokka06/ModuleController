using System.Collections;
using System.Collections.Generic;
using Demos.Common;
using UnityEngine;

namespace Demos.Vehicle
{
    public struct TargetData
    {
        public Vector3 NearestPoint;
        public Vector3 NearestForward;
        public Vector3 LookAheadPoint;
        public Vector3 LookAheadForward;
        public float Distance;
    }
    
    public class AIRaceModule : AbstractAIModule
    {
        [Tooltip("PID Controller values for smoothing out steering.")]
        public PIDValues SteerValues = new PIDValues(2f, 0f, 0.5f);
        public float MaxCutDistance = 5f;
        public float LookAheadTime = 0.5f;
        
        private VehicleSteerModule _steerModule;
        private PIDController _steerController;
        /// <summary>
        /// A lookup table for friction force from slip angle. 
        /// </summary>
        private AnimationCurve _slipForceCurve;
        
        private float _currentTF;
        
        public TargetData TargetData { get; private set; }
        public TargetData PrevTargetData { get; private set; }

        public override void SetupModule(DriverAIController controller)
        {
            base.SetupModule(controller);

            _steerController = new PIDController(SteerValues);
            _currentTF = Controller.PathManager.GetNearestPointTF(Controller.Vehicle.Controller.Transform.position);

            // Initialize slip force curve. used to brake on sharp turns.
            _slipForceCurve = new AnimationCurve();
            var fullLoad = Controller.Vehicle.Controller.Rigidbody.mass * Physics.gravity.magnitude / Controller.Vehicle.Controller.Wheels.Count;
            var angle = 0f;
            var interval = 10f;
            while (angle <= 90f)
            {
                var frictionForce = 0f;
                for (int i = 0; i < Controller.Vehicle.Controller.Wheels.Count; i++)
                {
                    Controller.Vehicle.Controller.Wheels[i].LateralFrictionModel
                        .GetLateral(fullLoad, angle, Time.fixedDeltaTime, out var force);
                    frictionForce += Mathf.Abs(force);
                }

                _slipForceCurve.AddKey(angle, frictionForce);
                angle += interval;
            }
        }

        public override void AssignVehicle(Vehicle vehicle)
        {
            base.AssignVehicle(vehicle);
            
            if (vehicle == null)
            {
                _steerModule = null;
                return;
            }
            
            _steerModule = vehicle.Controller.GetModule<VehicleSteerModule>();
        }

        public override void UpdateModule(float deltaTime)
        {
            if  (!Enabled)
                return;

            UpdateTarget(out var targetData);
            PrevTargetData = TargetData;
            TargetData = targetData;
            
            Controller.SetSteer(GetSteer());
            Controller.SetBrake(GetBrake());
            Controller.SetThrottle(GetThrottle());
        }
        
        private void UpdateTarget(out TargetData targetData)
        {
            var position = Controller.Vehicle.Controller.Transform.position;
            
            var nearestPointTF = Controller.PathManager.GetNearestPointTF(position);
            var lookAheadTF =
                Controller.PathManager.DistanceToTF(Mathf.Max(Controller.Vehicle.Controller.Rigidbody.velocity.magnitude * LookAheadTime, 1f));
            
            /*var deltaTF = nearestPointTF - _currentTF;
            
            if (Mathf.Sign(deltaTF) < 0)
            {
                //Debug.Log("<color=red>CANT GO BACK</color>");
            }
            var deltaDistance = Controller.PathManager.TFToDistance(deltaTF);
            if (deltaDistance > MaxCutDistance)
            {
                //Debug.Log("<color=blue>MASSIVE CUT</color>");
            }*/
            
            _currentTF = nearestPointTF;
            
            var nearestPoint = Controller.PathManager.Interpolate(_currentTF, out var nearestForward);
            var lookAheadPoint = Controller.PathManager.Interpolate(_currentTF + lookAheadTF, out var lookAheadForward);
            var distance = Controller.PathManager.TFToDistance(lookAheadTF);
            
            targetData = new TargetData
            {
                NearestPoint = nearestPoint,
                NearestForward = nearestForward,
                LookAheadPoint = lookAheadPoint,
                LookAheadForward = lookAheadForward,
                Distance = distance
            };
        }

        private float GetSteer()
        {
            var steer = 0f;

            var position = Controller.Vehicle.transform.position;
            var forward = Controller.Vehicle.Controller.Transform.forward;
            var forward2D = new Vector2(forward.x, forward.z).normalized;

            var velocity = Controller.Vehicle.Controller.Rigidbody.velocity;
            var velocity2D = new Vector2(velocity.x, velocity.z);
            var position2D = new Vector2(position.x, position.z);
            var lookAheadPoint2D = new Vector2(TargetData.LookAheadPoint.x, TargetData.LookAheadPoint.z);
            var lookAheadForward2D = new Vector2(TargetData.LookAheadForward.x, TargetData.LookAheadForward.z);
            var heading2D = (lookAheadPoint2D - position2D).normalized;

            var error = Mathf.Clamp(Vector2.SignedAngle(forward2D, heading2D), 
                -_steerModule.SteerAngle, _steerModule.SteerAngle);
            
            // Get an adjusting value from pid controller.
            var output = _steerController.GetOutput(error, 0f, Time.fixedDeltaTime);
            
            // clamp angle to steer angle and normalize it between -1f and 1f.
            steer = Mathf.Clamp(output, -_steerModule.SteerAngle, _steerModule.SteerAngle) / _steerModule.SteerAngle;

            return steer;

            /*
            var wheelBase = Vector3.Distance(Controller.Vehicle.Controller.Wheels[0].transform.localPosition, Controller.Vehicle.Controller.Wheels[2].transform.localPosition);
            var steerAngle = PurePursuitSteer(position2D, forward2D, lookAheadPoint2D, wheelBase);

            var pathPoint2D = new Vector2(TargetData.NearestPoint.x, TargetData.NearestPoint.z);
            var pathForward2D = new Vector2(TargetData.NearestForward.x, TargetData.NearestForward.z).normalized;
            var axleForward = Vector3.Slerp(Controller.Vehicle.Controller.Wheels[0].transform.forward,
                Controller.Vehicle.Controller.Wheels[1].transform.forward, 0.5f);
            var axleForward2D = new Vector2(axleForward.x, axleForward.z).normalized;
            var axleCenter = Vector3.Lerp(Controller.Vehicle.Controller.Wheels[0].transform.position,
                Controller.Vehicle.Controller.Wheels[1].transform.position, 0.5f);
            var axleCenter2D = new Vector2(axleCenter.x, axleCenter.z);
            steerAngle = StanleySteer(axleCenter2D, axleForward2D, pathPoint2D, pathForward2D,
                Controller.Vehicle.Controller.Rigidbody.velocity.magnitude, 1f);
            
            steer = Mathf.Clamp(steerAngle / _steerModule.SteerAngle, -1f, 1f);
            return steer;
            */
        }

        /// <summary>
        /// Calculates steer angle in degrees.
        /// </summary>
        /// <param name="axlePoint">Center axle world space position.</param>
        /// <param name="axleForward">Axle forward direction.</param>
        /// <param name="pathPoint">Closest point on path.</param>
        /// <param name="pathForward">Path forward direction.</param>
        /// <param name="velocity">Vehicle speed.</param>
        /// <param name="k">Steering gain coefficient between 0f-1f.</param>
        /// <returns></returns>
        private float StanleySteer(Vector2 axlePoint, Vector2 axleForward, Vector2 pathPoint, Vector2 pathForward, float velocity, float k = 1f)
        {
            var e = Vector2.Distance(axlePoint, pathPoint);
            
            var yaw = Mathf.Atan2(axleForward.y, axleForward.x);
            var pathYaw = Mathf.Atan2(pathForward.y, pathForward.x);
            var headingError = NormalizeRadian(pathYaw - yaw);
            
            /*
            var crosstrackYaw = Mathf.Atan2(axlePoint.y - pathPoint.y, axlePoint.x - pathPoint.x);
            var crosstrackError = NormalizeRadian(pathYaw - crosstrackYaw);
            e *= Mathf.Sign(crosstrackError);
            */
            
            var deltaError = Mathf.Atan2(velocity, k * e);
            
            return (headingError + deltaError) * Mathf.Rad2Deg;
            
            /*var headingError = Mathf.Clamp(Vector2.SignedAngle(axleForward, pathForward), -MaxAngle, MaxAngle);
            var deltaError = Mathf.Clamp(Mathf.Atan2(velocity + Mathf.Epsilon, k * e) * Mathf.Rad2Deg, -MaxAngle, MaxAngle);
            
            return headingError + deltaError;*/
        }

        private float NormalizeDegrees(float angle)
        {
            return angle - 180f * Mathf.Floor((angle + 180f) / 180f);
        }

        private float NormalizeRadian(float angle)
        {
            if (angle > Mathf.PI)
                angle -= 2f * Mathf.PI;
            if (angle < -Mathf.PI)
                angle += 2f * Mathf.PI;

            return angle;
        }

        private float PurePursuitSteer(Vector2 position, Vector2 forward, Vector2 lookAheadPoint, float wheelBase)
        {
            var alpha = Vector2.SignedAngle((lookAheadPoint - position).normalized, forward);
            var radius = wheelBase * 2f * Mathf.Sin(alpha * Mathf.Deg2Rad);
            var ld = Vector2.Distance(position, lookAheadPoint);
            return Mathf.Atan2(radius, ld) * Mathf.Rad2Deg;
        }
        
        private float GetThrottle()
        {
            // Prefer full throttle if no brake is used.
            var throttle = 1f;
            
            if (Controller.VehicleInputs.Brake > 0f)
                throttle = 0f;
            
            return throttle;
        }

        private float GetBrake()
        {
            var brake = 0f;

            var nearestPoint2D = new Vector2(TargetData.NearestPoint.x, TargetData.NearestPoint.z);
            var lookAheadPoint2D = new Vector2(TargetData.LookAheadPoint.x, TargetData.LookAheadPoint.z);
            
            var mass = Controller.Vehicle.Controller.Rigidbody.mass;
            var velocity = Controller.Vehicle.Controller.Rigidbody.velocity;
            var radius = GetCurvatureRadius(
                nearestPoint2D, 
                new Vector2(TargetData.NearestForward.x, TargetData.NearestForward.z).normalized, 
                lookAheadPoint2D);

            // Curve radius infinite, no need to brake.
            if (float.IsInfinity(radius))
                return 0f;
            
            var centripetalForce = mass * velocity.sqrMagnitude / radius;
            //Debug.Log($"radius: {radius}, centripetal force: {centripetalForce}");

            // Calculate curve angle from radius
            var angleA = Vector2.Distance(nearestPoint2D, lookAheadPoint2D) / radius * Mathf.Rad2Deg;
            
            var angleB = Vector2.Angle(
                new Vector2(velocity.x,velocity.z).normalized,
                lookAheadPoint2D);
            //Debug.Log($"{angleA}, {angleB}");
            
            var frictionForce = _slipForceCurve.Evaluate(angleB) * 2f;
            //Debug.Log($"Centripetal: {centripetalForce}, Friction: {frictionForce} = {Mathf.InverseLerp(1f, 2f, (centripetalForce / frictionForce))}");
            //Debug.Log($"angle: {angle}, friction force: {frictionForce}");

            brake = velocity.sqrMagnitude < 1f ? 0f : Mathf.Clamp01(Mathf.InverseLerp(1f, 2f, (centripetalForce / frictionForce)));
            return brake;
        }

        private float GetCurvatureRadius(Vector2 startPoint, Vector2 tangent, Vector2 endPoint)
        {
            var heading = (endPoint - startPoint).normalized;
            var dot = Vector2.Dot(tangent, heading);
            var distance = Vector2.Distance(startPoint, endPoint);
            var angle = Vector2.Angle(tangent, heading) * Mathf.Deg2Rad;

            return distance * 0.5f / Mathf.Sin(angle);
        }

        private void OnDrawGizmos()
        {
            if (!Application.isPlaying)
                return;

            var position2D = Controller.Vehicle.Controller.Position;
            position2D.y = TargetData.NearestPoint.y;
            
            Gizmos.DrawLine(position2D, TargetData.NearestPoint);
            Gizmos.DrawLine(position2D, TargetData.LookAheadPoint);
        }
    }
}