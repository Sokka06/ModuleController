using System;
using System.Collections;
using System.Collections.Generic;
using Demos;
using UnityEngine;

namespace Demos
{
    public struct CharacterInputs
    {
        public Vector2 Move;
        public bool Jump;
        public bool Sprint;
        public bool Crouch;
    }

    public class CharacterInputModule : AbstractCharacterModule
    {
        public CharacterInputs Inputs;
        public event Action onInput;
        
        public void SetInputs(ref CharacterInputs inputs)
        {
            Inputs = new CharacterInputs();

            if (Enabled)
            {
                Inputs = inputs;
                Inputs.Move = Vector2.ClampMagnitude(Inputs.Move, 1f);
            }
            
            onInput?.Invoke();
        }
        
        public override void UpdateModule(float deltaTime)
        {
            
        }
    }
}