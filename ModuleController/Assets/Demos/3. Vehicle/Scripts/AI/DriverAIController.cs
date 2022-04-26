using System;
using System.Collections;
using System.Collections.Generic;
using Sokka06.ModuleController;
using UnityEngine;

namespace Demos.Vehicle
{
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
        private float _deltaAccum;
        
        private VehicleInputModule _inputModule;
        
        public AbstractAIModule CurrentModule { get; private set; }
        public AIVehicleInputs VehicleInputs { get; private set; }
        public Vehicle Vehicle { get; private set; }
        public PathManager PathManager { get; private set; }
        
        public event Action<(AbstractAIModule prev, AbstractAIModule current)> onModuleChanged; 

        protected override void Awake()
        {
            base.Awake();

            SetModule(GetModule<AIRaceModule>());
            VehicleInputs = new AIVehicleInputs();
            PathManager = FindObjectOfType<PathManager>();
        }

        private void Start()
        {
            AssignVehicle(Driver.Vehicle);
            
            SetupModules();
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
            _deltaAccum += deltaTime;
            
            if (UpdateInterval > 0 && _updateCount % UpdateInterval > 0)
            {
                _updateCount++;
                return;
            }

            if (CurrentModule != null)
                CurrentModule.UpdateModule(_deltaAccum);
            
            _deltaAccum = 0f;
            _updateCount = 0;
        }

        public void SetModule(AbstractAIModule module)
        {
            var prevModule = CurrentModule;
            CurrentModule = module;
            
            if(prevModule != null)
                prevModule.ExitModule();
            if(CurrentModule != null)
                CurrentModule.EnterModule();
            
            onModuleChanged?.Invoke((prevModule, CurrentModule));
        }

        public void SetSteer(float value, float weight = 1f)
        {
            value = Mathf.Lerp(VehicleInputs.Steer, value, weight);
            
            VehicleInputs.Steer = value;
        }

        public void SetThrottle(float value, float weight = 1f)
        {
            value = Mathf.Lerp(VehicleInputs.Throttle, value, weight);
            
            VehicleInputs.Throttle = value;
        }

        public void SetBrake(float value, float weight = 1f)
        {
            value = Mathf.Lerp(VehicleInputs.Brake, value, weight);
            
            VehicleInputs.Brake = value;
        }
    }
}