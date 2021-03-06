using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demos.Vehicle
{
    public class DemoIngameModule : AbstractDemoModule
    {
        public CountdownUI CountdownUI;
        
        private Racer _playerRacer;

        public override void EnterModule()
        {
            base.EnterModule();

            // Assuming first racer is the player.
            _playerRacer = Controller.RaceController.CurrentRace.Racers[0];
            _playerRacer.onFinish += OnPlayerFinish;
            
            CountdownUI.StartCountdown(value =>
            {
                // Start race on last step
                if (value < CountdownUI.Steps.Count - 1)
                    return;
                
                Controller.RaceController.StartRace();
            
                // Unfreeze all racers.
                for (int i = 0; i < Controller.RaceController.CurrentRace.Racers.Count; i++)
                {
                    Controller.RaceController.CurrentRace.Racers[i].Driver.Vehicle.SetFreeze(false);
                }
            });
        }

        public override void ExitModule()
        {
            base.ExitModule();

            _playerRacer.onFinish -= OnPlayerFinish;
        }

        private void OnPlayerFinish()
        {
            Controller.SetState(Controller.GetModule<DemoFinishModule>());
        }
    }
}