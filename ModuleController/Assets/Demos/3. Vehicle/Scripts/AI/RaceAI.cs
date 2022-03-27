using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Demos.Vehicle
{
    public class RaceAI : MonoBehaviour
    {
        private VehicleInputModule _inputModule;
        
        public Vehicle Vehicle { get; private set; }
        public WaypointManager WaypointManager { get; private set; }
        public Waypoint Target { get; private set; }

        private float _throttleScale;

        public void Setup(Vehicle vehicle)
        {
            AssignVehicle(vehicle);

            WaypointManager = FindObjectOfType<WaypointManager>();
            Target = WaypointManager.Waypoints[0];
        }

        private void Start()
        {
            _throttleScale = Random.Range(0.75f, 1f);
        }

        public void AssignVehicle(Vehicle vehicle)
        {
            Vehicle = vehicle;
            if (Vehicle == null)
            {
                _inputModule = null;
                return;
            }
            _inputModule = Vehicle.Controller.GetModule<VehicleInputModule>();
        }

        private void FixedUpdate()
        {
            if (_inputModule == null)
                return;

            UpdateTarget();
            
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

        private void UpdateTarget()
        {
            var position = Vehicle.transform.position;
            var targetPosition = Target.transform.position;

            var reachedTarget = ReachedTarget(position, targetPosition, 5f);
            if (reachedTarget)
            {
                Target = WaypointManager.GetNext(Target);
            }
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
            
            var steer = Vector3.Dot(direction, right);
            return steer;
        }
        
        private float GetThrottle(Vector3 position, Vector3 target)
        {
            var throttle = 1f;

            return throttle * _throttleScale;
        }
        
        private float GetBrake(Vector3 position, Vector3 target)
        {
            var brake = 0f;

            return brake;
        }
    }
}