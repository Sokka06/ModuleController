using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demos.Vehicle
{
    [Serializable]
    public class Gear
    {
        public float Ratio;

        public Gear(float ratio)
        {
            Ratio = ratio;
        }
    }
    
    /// <summary>
    /// Basic gearbox.
    /// Index 0 should be reverse gear, index 1 neutral and next 1st, 2nd, 3rd etc
    /// </summary>
    public class Gearbox
    {
        public event Action<int> onGearChanged;
        
        public List<Gear> Gears { get; private set; }
        public int CurrentGearIndex { get; private set; }

        public Gearbox(List<Gear> gears, int initialGear = 0)
        {
            Gears = gears;
            CurrentGearIndex = Mathf.Clamp(initialGear, 0, gears.Count - 1);
        }

        public void ShiftGear(int dir)
        {
            if (dir == 0)
                return;

            dir = Mathf.Clamp(dir, -1, 1);
            
            SetGear(CurrentGearIndex + dir);
        }

        public void SetGear(int index)
        {
            index = Mathf.Clamp(index, 0, Gears.Count - 1);
            
            if (CurrentGearIndex == index)
                return;
            
            CurrentGearIndex = index;
            onGearChanged?.Invoke(CurrentGearIndex);
        }

        public float GetRatio()
        {
            return Gears[CurrentGearIndex].Ratio;
        }
    }
    
    public class VehicleGearboxModule : AbstractVehicleModule
    {
        [Header("Gearbox")]
        public List<Gear> Gears = new List<Gear>
        {
            new Gear(-0.5f), 
            new Gear(0f), 
            new Gear(1f),
        };
        public int InitialGear = 1;
        public float ShiftTime = 0.1f;
        public bool AutoReverse = true;

        private VehicleInputModule _inputModule;
        
        public Gearbox Gearbox { get; private set; }

        private void Awake()
        {
            Gearbox = new Gearbox(Gears, InitialGear);
        }

        public override void SetupModule(VehicleController controller)
        {
            base.SetupModule(controller);

            _inputModule = Controller.GetModule<VehicleInputModule>();
        }

        public override void UpdateModule(float deltaTime)
        {
            if (AutoReverse && Controller.GroundData.IsGrounded)
            {
                // holding brake, but going forward = don't shift to reverse
                // holding brake, but stopped = shift to reverse
                // holding throttle

                var forwardSpeed = Vector3.Dot(Controller.Rigidbody.velocity, Controller.Transform.forward);
                
                // In forward gear
                if (Gearbox.GetRatio() > 0f && Controller.Rigidbody.velocity.sqrMagnitude < 1f)
                {
                    if (_inputModule.Inputs.Brake > 0f)
                        Gearbox.SetGear(0);
                }
                
                // In reverse gear
                if (Gearbox.GetRatio() < 0f && _inputModule.Inputs.Throttle > 0f)
                {
                    Gearbox.SetGear(2);
                }
            }
        }
        
        public void Shift(int dir)
        {
            Gearbox.ShiftGear(dir);
        }
    }
}