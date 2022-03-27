using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Demos.Vehicle
{
    [Serializable]
    public struct DriverConfig
    {
        public AbstractDriver Driver;
        public Vehicle Vehicle;
    }
    
    public class DriverManager : MonoBehaviour
    {
        public SpawnManager SpawnManager;
        public List<DriverConfig> Drivers;

        public List<(AbstractDriver Driver, Vehicle Vehicle)> CurrentDrivers { get; private set; } = new List<(AbstractDriver driver, Vehicle vehicle)>();

        private void Awake()
        {
            for (int i = 0; i < Drivers.Count; i++)
            {
                SpawnManager.GetNext().GetSpawn(out var position, out var rotation);
                
                var driver = Instantiate(Drivers[i].Driver, transform);
                var vehicle = Instantiate(Drivers[i].Vehicle, position, rotation);
                
                CurrentDrivers.Add((driver, vehicle));
            }
        }

        private void Start()
        {
            for (int i = 0; i < CurrentDrivers.Count; i++)
            {
                CurrentDrivers[i].Driver.Setup(CurrentDrivers[i].Vehicle);
            }
        }
    }
}