using System;
using System.Collections;
using System.Collections.Generic;
using Sokka06.ModuleController;
using UnityEngine;

namespace Demos.Vehicle
{
    public struct GroundData
    {
        public bool IsGrounded;
        public float Factor;
        public Vector3 Normal;
        public Vector3 Velocity;
    }

    public struct VehicleFrame
    {
        /// <summary>
        /// Wheel base
        /// </summary>
        public float Width;
        /// <summary>
        /// Axle Length
        /// </summary>
        public float Length;

        public VehicleFrame(Transform parent, params CustomWheel[] wheels)
        {
            var shortest = float.MaxValue;
            var longest = float.MinValue;

            for (int i = 0; i < wheels.Length; i++)
            {
                var nextIndex = (i + 1) % wheels.Length;
            }
            
            for (int i = 0; i < wheels.Length; i++)
            {
                var p0 = new Vector2 (wheels[i].transform.localPosition.x, wheels[i].transform.localPosition.z);

                for (int j = 0; j < wheels.Length; j++)
                {
                    if (j == i)
                        continue;
                    
                    var p1 = new Vector2 (wheels[j].transform.localPosition.x, wheels[j].transform.localPosition.z);
                
                    var distance = Vector2.Distance(p0, p1);
                    var heading = (p1 - p0).normalized;
                    
                    var sideDot = Mathf.Abs(Vector2.Dot(heading, Vector2.right));
                    if (distance < shortest && sideDot > 0.9f)
                        shortest = distance;
                    
                    var upDot = Mathf.Abs(Vector2.Dot(heading, Vector2.up));
                    if (distance > longest && upDot > 0.9f)
                        longest = distance;
                }
            }

            Width = shortest;
            Length = longest;
        }

        public override string ToString()
        {
            return $"Width: {Width}, Length: {Length}";
        }
    }

    [DefaultExecutionOrder(-5)]
    public class VehicleController : ModuleControllerBehaviour<VehicleController, AbstractVehicleModule>
    {
        public Rigidbody Rigidbody;
        public List<CustomWheel> Wheels;

        public GroundData GroundData { get; private set; }
        public VehicleFrame Frame { get; private set; }
        public List<Collider> LocalColliders { get; private set; }
        
        public Transform Transform => Rigidbody.transform;
        public Vector3 Position => Rigidbody.position;
        public Quaternion Rotation => Rigidbody.rotation;
        public Vector3 Velocity => Rigidbody.velocity;
        public Vector3 LocalVelocity => Transform.InverseTransformVector(Velocity);
        public Vector3 PointVelocity(Vector3 point) => Rigidbody.GetPointVelocity(point);
        public Vector3 AngularVelocity => Rigidbody.angularVelocity;
        public float Mass => Rigidbody.mass;

        public event Action onModulesChanged;

        private void OnValidate()
        {
            if (Rigidbody == null)
                Rigidbody = transform.parent.GetComponent<Rigidbody>();

            if (!(Wheels.Count > 0))
                Wheels = new List<CustomWheel>(GetComponentsInChildren<CustomWheel>());
        }

        protected override void Awake()
        {
            base.Awake();
            
            GroundData = new GroundData();
            Frame = new VehicleFrame(transform, Wheels.ToArray());
            Debug.Log(Frame);
            LocalColliders = new List<Collider>(FindLocalColliders());
        }

        private void Start()
        {
            Rigidbody.maxAngularVelocity *= 2f;
            
            // Ignore collisions between wheels and local colliders.
            for (int i = 0; i < Wheels.Count; i++)
            {
                for (int j = 0; j < LocalColliders.Count; j++)
                {
                    Physics.IgnoreCollision(Wheels[i].Collider, LocalColliders[j]);
                }
            }

            SetupModules();
        }

        public override void SetupModules()
        {
            base.SetupModules();
            
            onModulesChanged?.Invoke();
        }

        private void FixedUpdate()
        {
            var deltaTime = Time.deltaTime;
            
            UpdateWheels(deltaTime);
            UpdateModules(deltaTime);
        }

        private void UpdateWheels(float deltaTime)
        {
            var isGrounded = false;
            var factor = 0f;
            var normalAvg = Vector3.zero;

            var invCount = 1f / Wheels.Count;
            
            for (int i = 0; i < Wheels.Count; i++)
            {
                Wheels[i].UpdateWheel(deltaTime);
                
                if (Wheels[i].GroundData.HasGround)
                {
                    isGrounded = true;
                    factor += invCount;
                    normalAvg += Wheels[i].GroundData.Hit.normal;
                }
            }
            
            normalAvg = (normalAvg * factor).normalized;
            
            var groundData = new GroundData
            {
                IsGrounded = isGrounded,
                Factor = factor,
                Normal = normalAvg,
            };
            GroundData = groundData;
        }

        private Collider[] FindLocalColliders()
        {
            var allColliders = new List<Collider>(Transform.GetComponentsInChildren<Collider>());
            var colliders = new List<Collider>();

            // Ignore wheel colliders
            for (int i = 0; i < allColliders.Count; i++)
            {
                //Debug.Log("Collider: " + allColliders[i].name);
                if (allColliders[i] is WheelCollider)
                {
                    //Debug.Log("Removed Wheel Collider");
                    continue;
                }
                
                colliders.Add(allColliders[i]);
            }
            
            return colliders.ToArray();
        }

        /// <summary>
        /// Sets rigidbody position.
        /// </summary>
        /// <param name="position"></param>
        public void SetPosition(Vector3 position)
        {
            Rigidbody.MovePosition(position);
        }
        
        /// <summary>
        /// Sets rigidbody rotation.
        /// </summary>
        /// <param name="rotation"></param>
        public void SetRotation(Quaternion rotation)
        {
            Rigidbody.MoveRotation(rotation);
        }

        public void SetPositionAndRotation(Vector3 position, Quaternion rotation)
        {
            SetPosition(position);
            SetRotation(rotation);
        }

        public void AddVelocity(Vector3 velocity, ForceMode forceMode = ForceMode.Force)
        {
            Rigidbody.AddForce(velocity, forceMode);
        }
        
        public void AddVelocity(Vector3 velocity, Vector3 point, ForceMode forceMode = ForceMode.Force)
        {
            Rigidbody.AddForceAtPosition(velocity, point, forceMode);
        }
        
        public void AddTorque(Vector3 torque, ForceMode forceMode = ForceMode.Force)
        {
            Rigidbody.AddTorque(torque, forceMode);
        }
    }
}