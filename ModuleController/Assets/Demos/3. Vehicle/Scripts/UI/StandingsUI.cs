using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demos.Vehicle
{
    public class StandingsUI : MonoBehaviour
    {
        public StandingUI StandingPrefab;

        private List<StandingUI> _standings = new List<StandingUI>();
        private RaceController _raceController;

        private void Start()
        {
            _raceController = FindObjectOfType<RaceController>();

            for (int i = 0; i < _raceController.CurrentRace.Standings.Standings.Count; i++)
            {
                var standing = Instantiate(StandingPrefab, transform.GetChild(0));
                standing.UpdateStanding((i + 1), _raceController.CurrentRace.Standings.Standings[i].Driver.Name);
                _standings.Add(standing);
            }
            
            _raceController.CurrentRace.Standings.onChanged += OnStandingsChanged;
        }

        private void OnDestroy()
        {
            _raceController.CurrentRace.Standings.onChanged -= OnStandingsChanged;
        }

        private void OnStandingsChanged()
        {
            for (int i = 0; i < _raceController.CurrentRace.Standings.Standings.Count; i++)
            {
                _standings[i].UpdateStanding(i + 1, _raceController.CurrentRace.Standings.Standings[i].Driver.Name);
            }
        }

        private void LateUpdate()
        {
            
        }
    }
}

