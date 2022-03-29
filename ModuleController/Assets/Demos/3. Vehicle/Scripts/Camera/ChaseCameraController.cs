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
        public DriverManager DriverManager;

        private int _targetIndex = 0;

        private void OnValidate()
        {
            if (VirtualCamera == null)
                VirtualCamera = GetComponent<CinemachineVirtualCameraBase>();
        }

        private void Start()
        {
            SetupModules();
            SetTarget(GetNextTarget(0));
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Z))
                SetTarget(GetNextTarget(-1));
            
            if (Input.GetKeyDown(KeyCode.X))
                SetTarget(GetNextTarget(1));
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

            var freeLook = (CinemachineFreeLook)VirtualCamera;
            freeLook.m_XAxis.Value = 0.5f;
        }

        private Transform GetNextTarget(int dir)
        {
            var lastIndex = DriverManager.CurrentDrivers.Count - 1;
            var index = _targetIndex + dir;
            
            if (index < 0)
                index = lastIndex;
            
            if (index > lastIndex)
                index = 0;

            _targetIndex = index;

            return DriverManager.CurrentDrivers[_targetIndex].Vehicle.transform;
        }
    }
}