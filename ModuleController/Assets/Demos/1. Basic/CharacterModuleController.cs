using System;
using System.Collections;
using System.Collections.Generic;
using Sokka06.ModuleController;
using UnityEngine;

namespace Demos
{
    public struct CharacterGroundData
    {
        public bool HasGround;
        public Vector3 Normal;
        public Vector3 Point;
    }
    
    /// <summary>
    /// Holds all modules, updates them and uses Character Controller to move and rotate the character.
    /// </summary>
    [DefaultExecutionOrder(-5)] //Makes sure Modules are setup before any other script tries to access them.
    public class CharacterModuleController : MonoBehaviour, IModuleController<CharacterModuleController, AbstractCharacterModule>
    {
        public CharacterController CharacterController;

        public List<AbstractCharacterModule> Modules { get; protected set; }
        
        public Vector3 Velocity { get; private set; }
        public Quaternion Rotation { get; private set; }

        public Transform Transform => CharacterController.transform;
        public HashSet<Collider> LocalColliders { get; private set; }
        public CharacterGroundData GroundData { get; private set; }

        private void OnValidate()
        {
            //To save a few seconds of my life
            if (CharacterController == null)
                CharacterController = transform.parent.GetComponent<CharacterController>();
        }

        private void Awake()
        {
            LocalColliders = new HashSet<Collider>(Transform.GetComponentsInChildren<Collider>());
            GroundData = new CharacterGroundData();

            SetRotation(Transform.rotation);
            
            SetupModules();
        }

        private void FixedUpdate()
        {
            var deltaTime = Time.deltaTime;

            var groundData = new CharacterGroundData();
            if (CharacterController.isGrounded)
                UpdateGround(out groundData);
            GroundData = groundData;

            //Update all modules.
            for (int i = 0; i < Modules.Count; i++)
            {
                Modules[i].UpdateModule(deltaTime);
            }

            //Move and rotate character.
            CharacterController.Move(Velocity * deltaTime);
            Transform.rotation = Rotation;
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

        /// <summary>
        /// Changes the height of the character.
        /// </summary>
        /// <param name="height"></param>
        public void SetHeight(float height)
        {
            height = Mathf.Max(height, CharacterController.radius * 2f);
            
            CharacterController.height = height;
            var newCenter = CharacterController.center;
            newCenter.y = height * 0.5f;
            CharacterController.center = newCenter;
        }

        /// <summary>
        /// Moves Character to given position. 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="resetVelocity"></param>
        /// <param name="resetRotation"></param>
        public void Teleport(Vector3 position, bool resetVelocity = false, bool resetRotation = false)
        {
            CharacterController.enabled = false;
            Transform.position = position;
            
            if(resetVelocity)
                SetVelocity(Vector3.zero);
            
            if(resetRotation)
                SetRotation(Quaternion.identity);
            
            CharacterController.enabled = true;
        }

        public void UpdateGround(out CharacterGroundData groundData)
        {
            var ray = new Ray(Transform.position + Transform.up * CharacterController.radius, -Transform.up);
            var results = new RaycastHit[LocalColliders.Count + 4];
            var hitCount =
                Physics.SphereCastNonAlloc(ray, CharacterController.radius, results, CharacterController.skinWidth * 2f);
                
            var hasGround = false;
            var normal = Transform.up;
            var point = Vector3.zero;

            var closestDistance = float.MaxValue;
            for (int i = 0; i < hitCount; i++)
            {
                if (LocalColliders.Contains(results[i].collider))
                    continue;

                if (closestDistance < results[i].distance)
                    continue;
                
                hasGround = true;
                normal = results[i].normal;
                point = results[i].point;
                closestDistance = results[i].distance;
                
                //
                var distanceDelta = Mathf.Abs(closestDistance - results[i].distance);
                if (distanceDelta < 0.01f)
                    continue;
            }

            groundData = new CharacterGroundData()
            {
                HasGround = hasGround,
                Normal = normal,
                Point = point
            };
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

        private void OnDrawGizmosSelected()
        {
            if (!GroundData.HasGround)
                return;
            
            var color = Color.blue;
            color.a *= 0.25f;
            Gizmos.color = color;
            
            Gizmos.DrawRay(GroundData.Point, GroundData.Normal);
            Gizmos.DrawSphere(GroundData.Point, 0.05f);
        }
    }
}