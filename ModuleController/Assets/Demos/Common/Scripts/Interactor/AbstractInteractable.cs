using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demos.Common
{
    public class AbstractInteractable : MonoBehaviour, IInteracteable
    {
        [Header("Interactable")] 
        public bool Enabled = true;
        public Collider Trigger;

        public event Action onInteract;

        private void Start()
        {
            Setup();
        }

        protected virtual void OnValidate()
        {
            if (Trigger == null)
                Trigger = GetComponent<Collider>();
            
            if (Trigger != null)
                Trigger.isTrigger = true;

            var targetLayer = LayerMask.NameToLayer("Interactable");
            if (gameObject.layer != targetLayer)
            {
                gameObject.layer = targetLayer;

                for (int i = 0; i < gameObject.transform.childCount; i++)
                {
                    gameObject.transform.GetChild(i).gameObject.layer = targetLayer;
                }
            }
        }

        public virtual void Setup()
        {
        
        }
    
        public virtual void Interact(AbstractInteractor interactor)
        {
            onInteract?.Invoke();
        }
    }
}