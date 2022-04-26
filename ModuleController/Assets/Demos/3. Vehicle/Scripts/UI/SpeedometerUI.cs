using System;
using System.Collections;
using System.Collections.Generic;
using Demos.Vehicle;
using TMPro;
using UnityEngine;

namespace Demos.Vehicle
{
    public class SpeedometerUI : MonoBehaviour
    {
        public TextMeshProUGUI SpeedometerText;

        private VehicleDriveModule _driveModule;

        private void Start()
        {
            var vehicle = FindObjectOfType<DriverManager>().CurrentDrivers[0].Vehicle;
            _driveModule = vehicle.Controller.GetModule<VehicleDriveModule>();
        }

        private void LateUpdate()
        {
            var speed = Vector3.Dot(_driveModule.Controller.Rigidbody.velocity, _driveModule.Controller.transform.forward);
            SpeedometerText.SetText($"{Mathf.Abs((int)MsToKPH(speed))} KPH");
        }

        private float MsToKPH(float ms)
        {
            return ms * 3.6f;
        }
    }
}