using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Demos.Demo1
{
    public class CameraLeanModule : AbstractCameraModule
    {
        [Header("Lean")]
        public AnimationCurve HorizontalCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
        public AnimationCurve VerticalCurve = AnimationCurve.Linear(0f, 0f, 1f, -0.2f);
        public float Sharpness = 10f;

        private float _inputSmoothed;

        public override void SetupModule(FirstPersonCameraController controller)
        {
            base.SetupModule(controller);
            
        }

        public override void UpdateModule(float deltaTime)
        {
            if (!Enabled)
                return;

            // TODO: Move to Input Module?
            var keyboard = Keyboard.current;
            if(keyboard == null)
                return;

            var leanInput = 0f;
            if (keyboard.qKey.isPressed)
                leanInput -= 1f;
            if (keyboard.eKey.isPressed)
                leanInput += 1f;

            _inputSmoothed = Mathf.Lerp(_inputSmoothed, leanInput, Sharpness * deltaTime);

            Offset = GetLeanOffset(_inputSmoothed);
        }

        private Vector3 GetLeanOffset(float input)
        {
            var absInput = Mathf.Abs(input);
            return new Vector3(HorizontalCurve.Evaluate(absInput) * Mathf.Sign(input), VerticalCurve.Evaluate(absInput));
        }
    }
}