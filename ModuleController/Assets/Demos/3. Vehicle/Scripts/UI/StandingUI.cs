using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Demos.Vehicle
{
    public class StandingUI : MonoBehaviour
    {
        public TextMeshProUGUI StandingText;
        public TextMeshProUGUI NameText;
        
        public void UpdateStanding(int standing, string driverName) 
        {
            StandingText.SetText($"{standing}.");
            NameText.SetText(driverName);
        }
    }
}

