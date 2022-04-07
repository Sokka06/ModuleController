using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Demos.Vehicle
{
    public class StatUI : MonoBehaviour
    {
        public TextMeshProUGUI TitleText;
        public TextMeshProUGUI ValueText;

        public string Title
        {
            get => TitleText.text;
            set => TitleText.SetText(value);
        }
    
        public string Value
        {
            get => ValueText.text;
            set => ValueText.SetText(value);
        }

        public void SetTitleAndValue(string title, string value)
        {
            Title = title;
            Value = value;
        }
    }
}

