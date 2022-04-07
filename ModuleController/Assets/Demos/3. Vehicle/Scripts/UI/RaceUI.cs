using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

namespace Demos.Vehicle
{
    public class RaceUI : MonoBehaviour
    {
        public TextMeshProUGUI LapText;
        public TextMeshProUGUI TimeText;

        [Space] 
        public float NumberSpacing = 32;
        public float DividerSpacing = 16;

        private DriverManager _driverManager;
        private RaceController _raceController;
        private Racer _racer;
        private bool _hasFinished;

        private void OnValidate()
        {
            if (TimeText == null)
                return;
            
            SetTime(0f);
        }

        private void Awake()
        {
            _driverManager = FindObjectOfType<DriverManager>();
            _raceController = FindObjectOfType<RaceController>();
        }

        private void Start()
        {
            // Assumes first racer is the player.
            _racer = _raceController.CurrentRace.Racers[0];
            UpdateLap();
            
            _racer.onLap += UpdateLap;
            _racer.onFinish += OnRacerFinish;
        }

        private void OnDestroy()
        {
            _racer.onLap -= UpdateLap;
            _racer.onFinish -= OnRacerFinish;
        }

        private void UpdateLap()
        {
            SetLap(_racer.LapData.Lap, _raceController.CurrentRace.Settings.Laps);
        }
        
        private void OnRacerFinish()
        {
            SetTime(_racer.FinishData.Time);
            _hasFinished = true;
        }

        private void LateUpdate()
        {
            if (!_hasFinished)
                SetTime(_raceController.CurrentRace.Data.Time);
        }

        private void SetTime(float time)
        {
            TimeText.SetText(time.FormatTime(NumberSpacing, DividerSpacing));
        }

        private void SetLap(int current, int total)
        {
            LapText.SetText($"Lap {Mathf.Min(current + 1, total)}<size=50%>/{total}</size>");
        }
    }
}

