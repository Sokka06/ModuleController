using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demos.Demo2
{
    /// <summary>
    /// Adjusts body drop based on feet target height to make sure both feet will reach the ground.
    /// </summary>
    public class HeightIKModule : AbstractIKModule
    {
        public Transform Root;
        public float MaxDrop = 0.5f;

        private FeetIKModule _feetIKModule;
        private Vector3 _targetLocalPosition;

        public override void SetupModule(IKModuleController controller)
        {
            base.SetupModule(controller);

            _feetIKModule = Controller.GetModule<FeetIKModule>();
        }

        public override void UpdateModule(float deltaTime)
        {
            
        }

        private void LateUpdate()
        {
            var lowestHeight = float.MaxValue;
            
            for (int i = 0; i < _feetIKModule.Feet.Count; i++)
            {
                if (!_feetIKModule.Feet[i].HasTarget)
                    continue;
                
                var position = _feetIKModule.Feet[i].TargetPosition;
                var localPosition = Root.InverseTransformPoint(position);
                Debug.Log($"{_feetIKModule.Feet[i].IKGoal}: {localPosition.y}");

                if (localPosition.y < lowestHeight)
                    lowestHeight = localPosition.y;
            }

            lowestHeight = Mathf.Clamp(lowestHeight, -MaxDrop, MaxDrop);

            var newPosition = Root.localPosition;
            newPosition.y = lowestHeight;
            _targetLocalPosition = newPosition;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawSphere(Root.TransformPoint(_targetLocalPosition), 0.05f);
        }
    }
}