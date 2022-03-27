using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Demos.Common;
using Sokka06.ModuleController;
using UnityEngine;

namespace Demos.Vehicle
{
    public class ChaseCameraController : ModuleControllerBehaviour<CameraController, AbstractCameraModule>
    {
        public CinemachineVirtualCameraBase VirtualCamera;

        private void OnValidate()
        {
            if (VirtualCamera == null)
                VirtualCamera = GetComponent<CinemachineVirtualCameraBase>();
        }

        private void Start()
        {
            SetupModules();
            SetTarget(FindObjectOfType<DriverManager>().CurrentDrivers[0].Vehicle.transform);
        }

        private void LateUpdate()
        {
            UpdateModules(Time.deltaTime);
        }

        public void SetTarget(Transform transform)
        {
            VirtualCamera.ForceCameraPosition(transform.position, transform.rotation);
            
            VirtualCamera.LookAt = transform;
            VirtualCamera.Follow = transform;
        }
    }
}