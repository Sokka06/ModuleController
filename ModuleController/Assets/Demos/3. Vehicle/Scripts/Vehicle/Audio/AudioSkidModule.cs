using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demos.Vehicle
{
    public class AudioSkidModule : AbstractAudioModule
    {
        public AnimationCurve VolumeCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
        public AnimationCurve PitchCurve = AnimationCurve.Linear(0f, 0.75f, 1f, 1.5f);
        
        private VehicleInputModule _inputModule;
        private VehicleDriveModule _driveModule;

        private float _pitch;
        private float _volume;

        public override void SetupModule(VehicleAudioController controller)
        {
            base.SetupModule(controller);

            _inputModule = Controller.VehicleController.GetModule<VehicleInputModule>();
            _driveModule = Controller.VehicleController.GetModule<VehicleDriveModule>();
        }

        public override void UpdateModule(float deltaTime)
        {
            var skidNormalized = 0f;

            var velocitySqrMag = Controller.VehicleController.Velocity.sqrMagnitude;

            if (Enabled && Controller.VehicleController.GroundData.IsGrounded && !(velocitySqrMag < 1f))
            {
                var forwardSpeed = Vector3.Dot(Controller.VehicleController.Velocity,
                    Controller.VehicleController.Transform.forward);
                var sideSpeed = Vector3.Dot(Controller.VehicleController.Velocity,
                    Controller.VehicleController.Transform.right);

                skidNormalized = sideSpeed * sideSpeed / velocitySqrMag;
            }
            
            _volume = VolumeCurve.Evaluate(skidNormalized);
            _pitch = PitchCurve.Evaluate(skidNormalized);
            
            Source.volume = _volume;
            Source.pitch = _pitch;
        }
    }
}