using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demos
{
    public enum CharacterAlignMode
    {
        Gravity,
        Surface
    }
    
    /// <summary>
    /// Aligns character with surface or gravity.
    /// </summary>
    public class CharacterAlignModule : AbstractCharacterModule
    {
        [Header("Align")] 
        public CharacterAlignMode AlignMode;
        public float Sharpness = 5f;

        private CharacterGravityModule _gravityModule;

        public override void SetupModule(CharacterModuleController controller)
        {
            base.SetupModule(controller);

            _gravityModule = Controller.GetModule<CharacterGravityModule>();
        }

        public override void UpdateModule(float deltaTime)
        {
            if (!Enabled)
                return;

            var targetUp = _gravityModule.Up;

            if (Controller.GroundData.HasGround)
            {
                targetUp = Controller.GroundData.Normal;
            }

            var currentUp = Controller.Rotation * Vector3.up;
            
            //Snippets from KinematicCharacterController (https://assetstore.unity.com/packages/tools/physics/kinematic-character-controller-99131)
            var smoothedGroundNormal = Vector3.Slerp(Controller.Transform.up, targetUp, 1 - Mathf.Exp(-Sharpness * deltaTime));
            Controller.SetRotation(Quaternion.FromToRotation(currentUp, smoothedGroundNormal) * Controller.Rotation);

            var initialCharacterBottomHemiCenter = Controller.Transform.position + (currentUp * Controller.CharacterController.radius);
            Controller.Transform.position = initialCharacterBottomHemiCenter +
                                            (Controller.Rotation * Vector3.down * Controller.CharacterController.radius);

            /*var targetRotation =
                Quaternion.FromToRotation(currentUp, targetUp) * Controller.Rotation;
            
            Controller.SetRotation(Quaternion.Slerp(Controller.Rotation, targetRotation, Sharpness * deltaTime));*/
        }
    }
}