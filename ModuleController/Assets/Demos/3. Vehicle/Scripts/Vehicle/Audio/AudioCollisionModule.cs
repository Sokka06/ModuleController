using System;
using System.Collections;
using System.Collections.Generic;
using Demos.Common;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Demos.Vehicle
{
    public class AudioCollisionModule : AbstractAudioModule, ICollisionCallbacks
    {
        public AudioClip[] Clips;
        public float MinImpulse = 1f;
        public float MaxImpulse = 10f;

        private CollisionListener _collisionListener;
        private float _invFixedDeltaTime;

        public override void SetupModule(VehicleAudioController controller)
        {
            base.SetupModule(controller);

            _collisionListener = Controller.VehicleController.Rigidbody.GetComponent<CollisionListener>();
            _collisionListener.Register(this);

            _invFixedDeltaTime = 1f / Time.fixedDeltaTime;
        }

        private void OnDestroy()
        {
            _collisionListener.Unregister(this);
        }

        public void CollisionEnter(Collision other)
        {
            var impulse = (other.impulse).magnitude * _invFixedDeltaTime;
            var mass = Controller.VehicleController.Mass;
            var min = MinImpulse * mass * _invFixedDeltaTime;
            var max = MaxImpulse * mass * _invFixedDeltaTime;
            
            if (!Enabled || !(Clips.Length > 0) || impulse < min)
                return;

            var volume = Mathf.InverseLerp(min, max, impulse);

            var clip = Clips[Random.Range(0, Clips.Length)];
            Source.transform.position = other.GetContact(0).point;
            Source.PlayOneShot(clip, volume);
            //AudioSource.PlayClipAtPoint(clip, other.GetContact(0).point, volume);
        }

        public void CollisionStay(Collision other)
        {
            
        }

        public void CollisionExit(Collision other)
        {
            
        }
    }
}

