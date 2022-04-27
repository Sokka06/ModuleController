using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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
        public List<CountdownStep> Steps;
        public TextMeshProUGUI Text;

        private void Awake()
        {
            Text.gameObject.SetActive(false);
        }

        public void StartCountdown(Action<int> onStep = null, Action onComplete = null)
        {
            StartCoroutine(Countdown(onStep, onComplete));
        }

        private IEnumerator Countdown(Action<int> onStep = null, Action onComplete = null)
        {
            Text.gameObject.SetActive(true);
            
            for (int i = 0; i < Steps.Count; i++)
            {
                onStep?.Invoke(i);
                Text.SetText($"<size={Steps[i].Scale*100}%>{Steps[i].Text}");
                yield return new WaitForSeconds(1f);
            }
            onComplete?.Invoke();
            
            Text.gameObject.SetActive(false);
        }
    }
}