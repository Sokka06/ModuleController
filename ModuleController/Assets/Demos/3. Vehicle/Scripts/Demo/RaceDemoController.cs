using System;
using System.Collections;
using System.Collections.Generic;
using Sokka06.ModuleController;
using UnityEngine;

namespace Demos.Vehicle
{
    public class RaceDemoController : ModuleControllerBehaviour<RaceDemoController, AbstractDemoModule>
    {
        public AbstractDemoModule[] StateModules;
        
        [Space]
        public List<DriverConfig> Drivers;
        public DriverManager DriverManager;
        public SpawnManager SpawnManager;
        public RaceController RaceController;
        
        public AbstractDemoModule State { get; private set; }
        public event Action<(AbstractDemoModule prevState, AbstractDemoModule currentState)> onStateChanged;

        protected override void Awake()
        {
            base.Awake();
            
            // Spawn drivers and vehicles.
            for (int i = 0; i < Drivers.Count; i++)
            {
                SpawnManager.GetNext().GetSpawn(out var position, out var rotation);
                
                var driver = Instantiate(Drivers[i].Driver, transform);
                var vehicle = Instantiate(Drivers[i].Vehicle, position, rotation);
                driver.Setup(vehicle);
                
                DriverManager.RegisterDriver((driver, vehicle));
            }
        }

        private void Start()
        {
            RaceController.CreateRace(RaceController.Settings, DriverManager.CurrentDrivers);
            
            SetupModules();
            SetState(Modules[0]);
        }

        private void Update()
        {
            State.UpdateModule(Time.deltaTime);
        }

        public override AbstractDemoModule[] FindModules()
        {
            return StateModules;
        }

        public void SetState(AbstractDemoModule state)
        {
            var prevState = State;
            if(prevState != null)
                prevState.ExitModule();
            
            State = state;
            if(State != null)
                State.EnterModule();
            
            onStateChanged?.Invoke((prevState, State));
        }
    }
}