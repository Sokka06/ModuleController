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
        public Vector3 Normal;
        public Vector3 Velocity;
    }

    public struct WheelData
    {
        public bool HasGround;
        public WheelHit Hit;
    }
    
    [DefaultExecutionOrder(-5)]
    public class VehicleController : ModuleControllerBehaviour<VehicleController, AbstractVehicleModule>
    {
        public Rigidbody Rigidbody;
        public List<WheelCollider> Wheels;

        public GroundData GroundData { get; private set; }
        public List<Collider> LocalColliders { get; private set; }
        public Transform Transform => Rigidbody.transform;

        public event Action onModulesChanged;

        private void OnValidate()
        {
            if (Rigidbody == null)
                Rigidbody = transform.parent.GetComponent<Rigidbody>();

            if (!(Wheels.Count > 0))
                Wheels = new List<WheelCollider>(GetComponentsInChildren<WheelCollider>());
        }

        protected override void Awake()
        {
            base.Awake();
            
            GroundData = new GroundData();

            LocalColliders = new List<Collider>(FindLocalCollider());

            for (int i = 0; i < Wheels.Count; i++)
            {
                for (int j = 0; j < LocalColliders.Count; j++)
                {
                    Physics.IgnoreCollision(Wheels[i], LocalColliders[j]);
                }
            }
        }

        private void Start()
        {
            SetupModules();
        }

        public override void SetupModules()
        {
            base.SetupModules();
            
            onModulesChanged?.Invoke();
        }

        private void FixedUpdate()
        {
            var groundData = new GroundData();
            var normalAvg = Vector3.zero;
            var count = 0;
            
            for (int i = 0; i < Wheels.Count; i++)
            {
                Wheels[i].motorTorque = 0.00001f;
                //Wheels[i].brakeTorque = 0.00001f;
                
                var hasGround = Wheels[i].GetGroundHit(out var wheelHit);
                if (hasGround)
                {
                    groundData.IsGrounded = true;
                    normalAvg += wheelHit.normal;
                    count++;
                }
            }

            if (count > 0)
                normalAvg = (normalAvg / count).normalized;

            groundData.Normal = normalAvg;
            
            GroundData = groundData;
            
            UpdateModules(Time.deltaTime);
        }

        private List<Collider> FindLocalCollider()
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
            
            return colliders;
        }
    }
}