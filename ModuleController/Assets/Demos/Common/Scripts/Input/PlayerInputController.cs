using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Demos;
using Sokka06.ModuleController;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Demos.Common
{
    public enum PlayerInputDevice
    {
        Any,
        KeyboardAndMouse,
        Gamepad
    }
    
    public class PlayerInputController : ModuleControllerBehaviour<PlayerInputController, AbstractInputModule>
    {
        public PlayerInputDevice InputDevice;

        public Mouse Mouse { get; private set; }
        public Keyboard Keyboard { get; private set; }
        public Gamepad Gamepad { get; private set; }
    
        private void Awake()
        {
            SetupModules();

            InputSystem.onAfterUpdate += OnAfterUpdate;
        }

        private void OnDestroy()
        {
            InputSystem.onAfterUpdate -= OnAfterUpdate;
        }

        private void OnAfterUpdate()
        {
            Mouse = Mouse.current;
            Keyboard = Keyboard.current;
            Gamepad = Gamepad.current;
            
            UpdateModules(Time.deltaTime);
        }
    }
}