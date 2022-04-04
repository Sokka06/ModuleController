using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demos.Vehicle
{
    public class DemoIngameModule : AbstractDemoModule
    {
        public RaceController RaceController;

        private Racer _playerRacer;

        public override void EnterModule()
        {
            base.EnterModule();
            
            RaceController.StartRace();
            
            // Un freeze all racers.
            for (int i = 0; i < RaceController.CurrentRace.Racers.Count; i++)
            {
                RaceController.CurrentRace.Racers[i].Driver.Vehicle.SetFreeze(false);
            }

            // Assuming first racer is the player.
            _playerRacer = RaceController.CurrentRace.Racers[0];
            _playerRacer.onFinish += OnPlayerFinish;
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