using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demos.Common
{
    [RequireComponent(typeof(Rigidbody))]
    public class CustomGravity : MonoBehaviour
    {
        public Rigidbody Rigidbody;
        public float Gravity = -9.81f;
    
        public Vector3 Up { get; set; }

        private void OnValidate()
        {
            if (Rigidbody == null)
                Rigidbody = GetComponent<Rigidbody>();

            Rigidbody.useGravity = false;
        }

        private void Start()
        {
            Up = Vector3.up;
        }

        private void FixedUpdate()
        {
            Rigidbody.AddForce(Up * Gravity * Rigidbody.mass);
        }
    }
}