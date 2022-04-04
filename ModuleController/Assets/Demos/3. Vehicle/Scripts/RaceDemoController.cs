using System;
using System.Collections;
using System.Collections.Generic;
using Sokka06.ModuleController;
using UnityEngine;

namespace Demos.Vehicle
{
    public enum RaceDemoState
    {
        Intro,
        Ingame,
        Finish
    }
    
    public class RaceDemoController : ModuleControllerBehaviour<RaceDemoController, AbstractDemoModule>
    {
        public AbstractDemoModule[] StateModules;
        
        public AbstractDemoModule State { get; private set; }
        public event Action<(AbstractDemoModule prevState, AbstractDemoModule currentState)> onStateChanged;

        protected override void Awake()
        {
            base.Awake();
        }

        private void Start()
        {
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