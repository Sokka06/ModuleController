using System;
using System.Collections;
using System.Collections.Generic;
using Sokka06.ModuleController;
using UnityEngine;

namespace Demos
{
    /// <summary>
    /// Holds all modules, updates them
    /// </summary>
    [DefaultExecutionOrder(-5)] //Make sure Modules are setup before any other script tries to access them.
    public class CharacterModuleController : MonoBehaviour, IModuleController<CharacterModuleController, AbstractCharacterModule>
    {
        public CharacterController CharacterController;

        public List<AbstractCharacterModule> Modules { get; protected set; }
        
        public Vector3 Velocity { get; private set; }
        public Quaternion Rotation { get; private set; }

        public Transform Transform => CharacterController.transform;
        public HashSet<Collider> LocalColliders { get; private set; }

        private void OnValidate()
        {
            //To save a few seconds of my life
            if (CharacterController == null)
                CharacterController = transform.parent.GetComponent<CharacterController>();
        }

        private void Awake()
        {
            LocalColliders = new HashSet<Collider>(Transform.GetComponentsInChildren<Collider>());

            SetRotation(CharacterController.transform.rotation);
            
            SetupModules();
        }

        private void FixedUpdate()
        {
            var deltaTime = Time.deltaTime;
            
            //unnecessary bug fix?
            /*if (CharacterController.isGrounded && Velocity.y < 0)
            {
                var velocity = Velocity;
                velocity.y = 0f;
                SetVelocity(velocity);
            }*/

            for (int i = 0; i < Modules.Count; i++)
            {
                Modules[i].UpdateModule(deltaTime);
            }

            CharacterController.Move(Velocity * deltaTime);
            CharacterController.transform.rotation = Rotation;
        }

        public void AddVelocity(Vector3 velocity)
        {
            Velocity += velocity;
        }
        
        public void SetVelocity(Vector3 velocity)
        {
            Velocity = velocity;
        }

        public void SetRotation(Quaternion rotation)
        {
            Rotation = rotation;
        }

        public void SetHeight(float height)
        {
            height = Mathf.Max(height, CharacterController.radius * 2f);
            
            CharacterController.height = height;
            var newCenter = CharacterController.center;
            newCenter.y = height * 0.5f;
            CharacterController.center = newCenter;
        }

        #region ModuleController
        public void SetupModules()
        {
            Modules = new List<AbstractCharacterModule>(FindModules());
            
            for (int i = 0; i < Modules.Count; i++)
            {
                Modules[i].SetupModule(this);
            }
        }

        public AbstractCharacterModule[] FindModules()
        {
            // Define where Modules are found.
            // In this case they're simply placed under this same GameObject and found with GetComponents.
            // You can also, for example, use GetComponentsInChildren if they're placed in a child object or another list that is manually populated in inspector
            
            return GetComponents<AbstractCharacterModule>();
        }

        public T GetModule<T>() where T : AbstractCharacterModule
        {
            // Finds the module for you and returns it. Returns null if module is not on the list.
            for (int i = 0; i < Modules.Count; i++)
            {
                if (Modules[i] is T module)
                    return module;
            }

            return null;
        }
        #endregion
    }
}