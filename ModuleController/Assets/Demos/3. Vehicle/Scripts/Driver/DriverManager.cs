using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Demos.Vehicle
{
    public class DriverManager : MonoBehaviour
    {
        public List<(AbstractDriver Driver, Vehicle Vehicle)> CurrentDrivers { get; private set; } = new List<(AbstractDriver driver, Vehicle vehicle)>();

        public void RegisterDriver((AbstractDriver driver, Vehicle vehicle) driver)
        {
            CurrentDrivers.Add(driver);
        }
    }
}