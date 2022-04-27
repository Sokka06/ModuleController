using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Demos.Vehicle
{
    [Serializable]
    public class CountdownStep
    {
        public string Text;
        public float Scale = 1f;
    }

    public struct Countdown
    {
        public readonly CountdownStep[] Steps;
        public readonly Action onStepComplete;
        public readonly Action onCountdownComplete;
    }
    
    public class CountdownUI : MonoBehaviour
    {
        public TextMeshProUGUI Text;
        public Image Image;
        public List<CountdownStep> Steps;

        private Color _initialColor;

        private void Awake()
        {
            _initialColor = Image.color;
            
            Text.gameObject.SetActive(false);
            Image.gameObject.SetActive(false);
        }

        public void StartCountdown(Action<int> onStep = null, Action onComplete = null)
        {
            StartCoroutine(Countdown(onStep, onComplete));
        }

        private IEnumerator Countdown(Action<int> onStep = null, Action onComplete = null)
        {
            Text.gameObject.SetActive(true);
            Image.gameObject.SetActive(true);
            Image.color = _initialColor;

            for (int i = 0; i < Steps.Count; i++)
            {
                onStep?.Invoke(i);
                Text.SetText($"<size={Steps[i].Scale*100}%>{Steps[i].Text}");

                if (i == Steps.Count - 1)
                {
                    var color = _initialColor;
                    color.a = (1f - Mathf.Lerp(0f, 1f, (float)i / Steps.Count)) * _initialColor.a;
                    Image.CrossFadeColor(color, 1f, false, true);
                }
                
                yield return new WaitForSeconds(1f);
            }
            onComplete?.Invoke();
            
            Text.gameObject.SetActive(false);
            Image.gameObject.SetActive(false);
        }
    }
}