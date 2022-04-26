using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

namespace Demos.Vehicle
{
    public class FinishCameraController : MonoBehaviour
    {
        public CinemachineVirtualCameraBase VirtualCamera;
        public DriverManager DriverManager;
        
        private void OnValidate()
        {
            if (VirtualCamera == null)
                VirtualCamera = GetComponent<CinemachineVirtualCameraBase>();
        }
        
        private void OnEnable()
        {
            // Assuming first racer is the player.
            var target = DriverManager.CurrentDrivers[0].Vehicle.transform;
            VirtualCamera.Follow = target;
            VirtualCamera.LookAt = target;
        }
    }
}