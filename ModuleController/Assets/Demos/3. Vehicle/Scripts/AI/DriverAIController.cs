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
    
    public class DriverAIController : ModuleControllerBehaviour<DriverAIController, AbstractAIModule>
    {
        public ComputerDriver Driver;
        public int UpdateInterval = 2;

        private int _updateCount;
        
        public Vehicle Vehicle { get; private set; }

        public AIState State { get; private set; }
        public WaypointManager WaypointManager { get; private set; }

        private void Start()
        {
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
        }

        private void FixedUpdate()
        {
            UpdateModules(Time.fixedDeltaTime);
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
    }
}