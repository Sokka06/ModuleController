using System.Collections;
using System.Collections.Generic;
using Demos.Demo1;
using UnityEngine;

namespace Demos.Demo1
{
    /// <summary>
    /// Aligns view to the ground normal.
    /// </summary>
    public class ViewAlignModule : AbstractViewModule
    {
        [Header("Align")]
        public Transform Root;
        public float Sharpness = 10f;

        public override void UpdateModule(float deltaTime)
        {
            if (!Enabled)
                return;

            var targetUp = Vector3.up;

            if (Controller.Character.GroundData.HasGround)
            {
                targetUp = Controller.Character.GroundData.Normal;
            }

            var currentUp = Root.rotation * Vector3.up;
            
            //Snippets from KinematicCharacterController (https://assetstore.unity.com/packages/tools/physics/kinematic-character-controller-99131)
            var smoothedGroundNormal = Vector3.Slerp(Root.up, targetUp, 1 - Mathf.Exp(-Sharpness * deltaTime));
            var forward = Vector3.Cross(-smoothedGroundNormal, Controller.Character.Transform.right);
            Root.rotation = Quaternion.LookRotation(forward, smoothedGroundNormal);

            var bottomHemiCenter = Root.position + (currentUp * Controller.Character.CharacterController.radius);
            Root.position = (bottomHemiCenter + (Root.rotation * Vector3.down * Controller.Character.CharacterController.radius));
        }
    }
}