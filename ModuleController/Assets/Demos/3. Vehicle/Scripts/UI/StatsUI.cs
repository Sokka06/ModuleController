using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demos.Vehicle
{
    public class StatsUI : MonoBehaviour
    {
        [Header("Stats")] 
        public StatUI StatPrefab;
        public RectTransform Container;
        
        private void Start()
        {
            var raceController = FindObjectOfType<RaceController>();
            // Assuming first racer is the player.
            var playerRacer = raceController.CurrentRace.Racers[0];
            var raceTimeStat = InstantiateStat("Race Time:", playerRacer.FinishData.Time.FormatTime(34f));
            var bestLapStat = InstantiateStat("Best Lap:", playerRacer.GetBestLapTime().FormatTime(34f));
        }

        private StatUI InstantiateStat(string title, string value)
        {
            var stat = Instantiate(StatPrefab, Container);
            stat.SetTitleAndValue(title, value);
            return stat;
        }
    }
}