using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demos.Vehicle
{
    public class AudioEngineModule : AbstractAudioModule
    {
        [Header("Engine")]
        public AnimationCurve VolumeCurve = AnimationCurve.Linear(0f, 0.5f, 1f, 1f);
        public float VolumeSharpness = 10f;
        
        [Space]
        public AnimationCurve PitchCurve = AnimationCurve.Linear(0f, 0.75f, 1f, 1.25f);
        public float PitchSharpness = 10f;
        
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
            var forwardSpeed = Vector3.Dot(Controller.VehicleController.Velocity,
                Controller.VehicleController.Transform.forward);
            var sideSpeed = Vector3.Dot(Controller.VehicleController.Velocity,
                Controller.VehicleController.Transform.right);
            var speedNormalized = Mathf.Abs(forwardSpeed) / _driveModule.Speed;
            
            var load = _inputModule.Inputs.Throttle;
            var loadExtra = Mathf.Lerp(-0.1f, 0.1f, load);

            _volume = VolumeCurve.Evaluate(speedNormalized) + loadExtra;
            _pitch = PitchCurve.Evaluate(speedNormalized) + loadExtra;
            
            Source.volume = _volume;
            Source.pitch = _pitch;
        }
    }
}