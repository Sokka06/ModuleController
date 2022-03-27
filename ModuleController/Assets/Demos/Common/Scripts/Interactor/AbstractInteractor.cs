using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demos.Common
{
    public enum InteractorTriggerMode
    {
        All,
        Enter,
        Stay,
        Exit
    }

    public abstract class AbstractInteractor : MonoBehaviour
    {
        public Collider Trigger;
        public InteractorTriggerMode TriggerMode;

        public Action<IInteracteable> onInteract;
        
        protected virtual void OnValidate()
        {
            Trigger = GetComponent<Collider>();

            if (Trigger != null)
                Trigger.isTrigger = true;
        }
        
        // Start is called before the first frame update
        void Awake()
        {
            Setup();
        }

        public virtual void Setup()
        {
            
        }

        private void OnTriggerEnter(Collider other)
        {
            if (TriggerMode == InteractorTriggerMode.All || TriggerMode == InteractorTriggerMode.Enter)
                InteractableEnter(FindInteractable(other.gameObject));
        }

        private void OnTriggerStay(Collider other)
        {
            if (TriggerMode == InteractorTriggerMode.All || TriggerMode == InteractorTriggerMode.Stay)
                InteractableStay(FindInteractable(other.gameObject));
        }

        private void OnTriggerExit(Collider other)
        {
            if (TriggerMode == InteractorTriggerMode.All || TriggerMode == InteractorTriggerMode.Exit)
                InteractableExit(FindInteractable(other.gameObject));
        }

        public abstract void InteractableEnter(IInteracteable interacteable);

        public abstract void InteractableStay(IInteracteable interacteable);

        public abstract void InteractableExit(IInteracteable interacteable);

        /// <summary>
        /// Finds IInteractable on object.
        /// </summary>
        protected IInteracteable FindInteractable(GameObject target)
        {
            //check target
            var interactable = target.GetComponent<IInteracteable>();
            if (interactable != null)
                return interactable;
            
            //check parent
            interactable = target.transform.parent.GetComponent<IInteracteable>();
            if (interactable != null)
                return interactable;
            
            //check parent
            interactable = target.transform.root.GetComponent<IInteracteable>();
            if (interactable != null)
                return interactable;

            return null;
        }

        public virtual bool CanInteract()
        {
            return true;
        }
    }
}