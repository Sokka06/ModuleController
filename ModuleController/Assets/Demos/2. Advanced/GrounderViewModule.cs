using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demos.Demo2
{
    public class GrounderViewModule : AbstractViewModule
    {
        public Animator Animator;
        public Transform Root;
        public List<Transform> Feet;
        
        public override void UpdateModule(float deltaTime)
        {
            base.UpdateModule(deltaTime);
            
            var lowestHeight = 0f;
            
            for (int i = 0; i < Feet.Count; i++)
            {
                if (Physics.Raycast(Feet[i].position + Controller.Character.Transform.up * 1f,
                    -Controller.Character.Transform.up, out var hit, 1f * 2f))
                {
                    var position = hit.point;
                    var localPosition = Root.InverseTransformPoint(position);

                    if (localPosition.y < lowestHeight)
                        lowestHeight = localPosition.y;
                }
            }

            lowestHeight = Mathf.Clamp(lowestHeight, -1f, 0f);

            var newPosition = Root.localPosition;
            newPosition.y = lowestHeight;
            Root.localPosition = newPosition;
        }
    }
}
