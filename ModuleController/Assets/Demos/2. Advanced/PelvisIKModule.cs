using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demos.Demo2
{
    /// <summary>
    /// Adjusts pelvis height based on feet target height to make sure both feet will reach the ground.
    /// </summary>
    public class PelvisIKModule : AbstractIKModule
    {
        public Transform Root;
        public float MaxDrop = 0.5f;
        public float Smoothness = 25f;

        private FeetIKModule _feetIKModule;
        private Vector3 _targetLocalPosition;
        private float _height;

        public override void SetupModule(IKModuleController controller)
        {
            base.SetupModule(controller);

            _feetIKModule = Controller.GetModule<FeetIKModule>();
        }

        public override void UpdateModule(float deltaTime)
        {
            var offset = 0f;
            
            for (int i = 0; i < _feetIKModule.Feet.Count; i++)
            {
                if (!Controller.Character.GroundData.HasGround)
                    break;
                
                if (!_feetIKModule.Feet[i].HasTarget)
                {
                    offset = -MaxDrop;
                    continue;
                }
                
                var position = _feetIKModule.Feet[i].TargetPosition;
                var footOffset = position.y - Controller.Listener.Animator.transform.position.y;
                var localPosition = Root.InverseTransformPoint(position);

                if (footOffset < offset)
                    offset = footOffset;
            }

            //Get hips position
            var newPosition = Controller.Listener.Animator.bodyPosition;
            //var newHeight = offset;
            //newPosition.y += offset;
            var targetHeight = newPosition.y + offset;
            _height = Mathf.Lerp(_height, targetHeight, Smoothness * deltaTime);
            newPosition.y = _height;
            Controller.Listener.Animator.bodyPosition = newPosition;

            /*var newPosition = Root.localPosition;
            newPosition.y = lowestHeight;
            _targetLocalPosition = newPosition;
            Root.localPosition = _targetLocalPosition;*/
        }

        private void LateUpdate()
        {
            
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawSphere(Root.TransformPoint(_targetLocalPosition), 0.05f);
        }
    }
}