using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Demos.Common
{
    [RequireComponent(typeof(SphereCollider))]
    public class AreaInteractor : AbstractInteractor
    {
        public float Radius = 1f;

        protected override void OnValidate()
        {
            base.OnValidate();
            
            var sphereTrigger = Trigger as SphereCollider;
        
            if(sphereTrigger != null)
                sphereTrigger.radius = Radius;
        }

        public override void Setup()
        {
            base.Setup();
        
        }

        public override void InteractableEnter(IInteracteable interacteable)
        {
            interacteable?.Interact(this);
        }

        public override void InteractableStay(IInteracteable interacteable)
        {
        
        }

        public override void InteractableExit(IInteracteable interacteable)
        {
        
        }

        public override bool CanInteract()
        {
            return true;
        }
    }
}