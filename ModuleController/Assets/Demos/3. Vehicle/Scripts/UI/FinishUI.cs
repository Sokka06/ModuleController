using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Demos.Vehicle
{
    public class FinishUI : MonoBehaviour
    {
        public TextMeshProUGUI FinishText;

        private void Start()
        {
            var raceController = FindObjectOfType<RaceController>();
            // Assuming first racer is the player.
            var standing = raceController.CurrentRace.Standings.GetStanding(raceController.CurrentRace.Racers[0]);
            FinishText.SetText($"Finished {ColorText(StandingToText(standing), StandingToColor(standing))}!");
        }

        private string StandingToText(int standing)
        {
            switch (standing)
            {
                case 1:
                    return $"{standing}st";
                case 2:
                    return $"{standing}nd";
                case 3:
                    return $"{standing}rd";
                default:
                    return $"{standing}th";
            }
        }

        private string StandingToColor(int standing)
        {
            switch (standing)
            {
                case 1:
                    return "yellow";
                case 2:
                    return "grey";
                case 3:
                    return "lightblue";
                default:
                    return "white";
            }
        }

        private string ColorText(string text, string color)
        {
            return $"<color={color}>{text}</color>";
        }
    }
}

