using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demos
{
    /// <summary>
    /// Adds drag to the Character, in other words slows it down.
    /// </summary>
    public class CharacterDragModule : AbstractCharacterModule
    {
        [Header("Drag")]
        public float Drag = 0.1f;
        
        public override void UpdateModule(float deltaTime)
        {
            if (!Enabled)
                return;
            
            Controller.SetVelocity(Controller.Velocity * (1f / (1f + (Drag * deltaTime))));
        }
    }
}