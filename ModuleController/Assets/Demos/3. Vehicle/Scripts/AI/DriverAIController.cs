using System;
using System.Collections;
using System.Collections.Generic;
using Sokka06.ModuleController;
using UnityEngine;

namespace Demos.Vehicle
{
    public enum AIState
    {
        Race,
        OffTrack,
        Stuck
    }

    public class AIVehicleInputs
    {
        public float Steer;
        public float Throttle;
        public float Brake;
    }
    
    public class DriverAIController : ModuleControllerBehaviour<DriverAIController, AbstractAIModule>
    {
        public ComputerDriver Driver;
        public int UpdateInterval = 2;

        private int _updateCount;
        private VehicleInputModule _inputModule;
        
        public AIVehicleInputs VehicleInputs { get; private set; }
        public Vehicle Vehicle { get; private set; }

        public AIState State { get; private set; }
        public WaypointManager WaypointManager { get; private set; }

        private void Start()
        {
            VehicleInputs = new AIVehicleInputs();
            State = AIState.Race;
            AssignVehicle(Driver.Vehicle);
            WaypointManager = FindObjectOfType<WaypointManager>();
            
            SetupModules();
        }

        public override void SetupModules()
        {
            base.SetupModules();
        }
        
        public void AssignVehicle(Vehicle vehicle)
        {
            Vehicle = vehicle;

            for (int i = 0; i < Modules.Count; i++)
            {
                Modules[i].AssignVehicle(vehicle);
            }
            
            if (vehicle == null)
            {
                _inputModule = null;
                return;
            }
            
            _inputModule = vehicle.Controller.GetModule<VehicleInputModule>();
        }

        private void FixedUpdate()
        {
            UpdateModules(Time.fixedDeltaTime);

            var inputs = new VehicleInputs
            {
                Steer = VehicleInputs.Steer,
                Throttle = VehicleInputs.Throttle,
                Brake = VehicleInputs.Brake
            };
            _inputModule.SetInputs(ref inputs);
        }

        public override void UpdateModules(float deltaTime)
        {
            if (UpdateInterval > 0 && _updateCount % UpdateInterval > 0)
            {
                _updateCount++;
                return;
            }
            
            for (int i = 0; i < Modules.Count; i++)
            {
                Modules[i].UpdateState(deltaTime);
            }
            
            base.UpdateModules(deltaTime);
        }

        public void SetSteer(float value, float weight = 1f)
        {
            if (weight > 0f)
            {
                value = Mathf.Lerp(VehicleInputs.Steer, value, weight);
            }
            
            VehicleInputs.Steer = value;
        }

        public void SetThrottle(float value, float weight = 1f)
        {
            if (weight > 0f)
            {
                value = Mathf.Lerp(VehicleInputs.Throttle, value, weight);
            }
            
            VehicleInputs.Throttle = value;
        }

        public void SetBrake(float value, float weight = 1f)
        {
            if (weight > 0f)
            {
                value = Mathf.Lerp(VehicleInputs.Brake, value, weight);
            }
            
            VehicleInputs.Brake = value;
        }
    }
}