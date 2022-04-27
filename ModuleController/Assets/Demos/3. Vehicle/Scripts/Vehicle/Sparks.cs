using System;
using System.Collections;
using System.Collections.Generic;
using Demos.Common;
using UnityEngine;

namespace Demos.Vehicle
{
    public class Sparks : MonoBehaviour, ICollisionCallbacks
    {
        public float ContactOffset = 0.05f;
        public float MinVelocity = 1f;
        public List<ParticleSystem> SparksParticleSystems;
        public CollisionListener CollisionListener;

        private void OnEnable()
        {
            CollisionListener.Register(this);
        }

        private void OnDisable()
        {
            CollisionListener.Unregister(this);
        }

        public void CollisionEnter(Collision other)
        {
        }

        public void CollisionStay(Collision other)
        {
            for (int i = 0; i < SparksParticleSystems.Count; i++)
            {
                var dot = Vector3.Dot(other.relativeVelocity.normalized, other.GetContact(i).normal);
                
                if (i >= other.contactCount || other.relativeVelocity.sqrMagnitude < MinVelocity * MinVelocity || Mathf.Abs(dot) > 0.25f)
                {
                    SparksParticleSystems[i].Stop();
                    continue;
                }
            
                var forward = other.relativeVelocity.normalized;
                var up = Vector3.Cross(forward, other.GetContact(i).normal);
                var rot = Quaternion.LookRotation(forward, up);
            
                SparksParticleSystems[i].transform.SetPositionAndRotation(other.GetContact(i).point + other.GetContact(i).normal * ContactOffset, rot);
                //SparksParticleSystems[i].transform.position = _currentContacts[i].point + _currentContacts[i].normal * ContactOffset;
                SparksParticleSystems[i].Play();
            }
        }

        public void CollisionExit(Collision other)
        {
            for (int i = 0; i < SparksParticleSystems.Count; i++)
            {
                SparksParticleSystems[i].Stop();
            }
        }
    }
}

