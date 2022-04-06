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
            var driver = _driverManager.CurrentDrivers[0].Driver;
            _racer = _raceController.CurrentRace.GetRacer(driver);
            OnRacerLap();
            
            _racer.onLap += OnRacerLap;
            _racer.onFinish += OnRacerFinish;
        }

        private void OnDestroy()
        {
            _racer.onLap -= OnRacerLap;
            _racer.onFinish -= OnRacerFinish;
        }

        private void OnRacerLap()
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
            //SetLap(_racer.PositionData.CurrentLap, _raceController.CurrentRace.Settings.Laps);
            if (!_hasFinished)
                SetTime(_raceController.CurrentRace.Data.Time);
        }

        private void SetTime(float time)
        {
            TimeText.SetText(FormatTime(time, NumberSpacing, DividerSpacing));
        }

        private void SetLap(int current, int total)
        {
            LapText.SetText($"Lap {Mathf.Min(current + 1, total)}<size=50%>/{total}</size>");
        }
        
        public string FormatTime(float time, float numberSpacing, float dividerSpacing)
        {
            var timespan = TimeSpan.FromSeconds(time);
            var tenth = Mathf.Floor(timespan.Milliseconds * 0.1f);

            var builder = new StringBuilder();
            builder.Append($"<mspace={numberSpacing}px>{timespan.Minutes:00}</mspace>");
            builder.Append($"<mspace={dividerSpacing}px>:</mspace>");
            builder.Append($"<mspace={numberSpacing}px>{timespan.Seconds:00}</mspace>");
            builder.Append($"<mspace={dividerSpacing}px>,</mspace>");
            builder.Append($"<mspace={numberSpacing}px>{tenth:00}</mspace>");

            return builder.ToString();

            /*return $"<mspace={numberSpacing}px>{timespan.Minutes:00}</mspace>" +
                   $"<mspace={dividerSpacing}px>:</mspace>" +
                   $"<mspace={numberSpacing}px>{timespan.Seconds:00}</mspace>" +
                   $"<mspace={dividerSpacing}px>:</mspace>" +
                   $"<mspace={numberSpacing}px>{tenth:00}</mspace>";*/
        }
    }
}

