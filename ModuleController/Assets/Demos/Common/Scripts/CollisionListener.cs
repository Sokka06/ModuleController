using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demos.Common
{
    public interface ICollisionCallbacks
    {
        public void CollisionEnter(Collision other);
        public void CollisionStay(Collision other);
        public void CollisionExit(Collision other);
    }
    
    /// <summary>
    /// Add this to a GameObject with a Rigidbody and register your callbacks to receive collision events.
    /// </summary>
    public class CollisionListener : MonoBehaviour
    {
        public Rigidbody Rigidbody;
        public List<ICollisionCallbacks> Callbacks { get; private set; }

        private void OnValidate()
        {
            if (Rigidbody == null)
                Rigidbody = GetComponent<Rigidbody>();
        }

        private void Awake()
        {
            Callbacks = new List<ICollisionCallbacks>();
        }

        public void Register(ICollisionCallbacks callbacks)
        {
            Callbacks.Add(callbacks);
        }
        
        public void Unregister(ICollisionCallbacks callbacks)
        {
            Callbacks.Remove(callbacks);
        }

        private void OnCollisionEnter(Collision other)
        {
            for (int i = 0; i < Callbacks.Count; i++)
            {
                Callbacks[i].CollisionEnter(other);
            }
        }
        
        private void OnCollisionStay(Collision other)
        {
            for (int i = 0; i < Callbacks.Count; i++)
            {
                Callbacks[i].CollisionStay(other);
            }
        }
        
        private void OnCollisionExit(Collision other)
        {
            for (int i = 0; i < Callbacks.Count; i++)
            {
                Callbacks[i].CollisionExit(other);
            }
        }
    }
}


