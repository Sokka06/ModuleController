using System;
using System.Collections;
using System.Collections.Generic;
using Demos.Common;
using UnityEditor;
using UnityEngine;

namespace Demos.Vehicle
{
    public class Checkpoint : AbstractInteractable
    {
        [Header("Checkpoint")]
        public bool OneWay;

        public event Action<Checkpoint, Vehicle> onCheckpoint;

        protected override void OnValidate()
        {
            base.OnValidate();
        }

        public override void Interact(AbstractInteractor interactor)
        {
            base.Interact(interactor);

            ProcessCheckpoint(interactor.transform);
        }

        private void ProcessCheckpoint(Transform interactor)
        {
            if(!CheckDirection(interactor))
                return;

            var vehicle = interactor.root.GetComponent<Vehicle>();
            if(vehicle == null)
                return;

            onCheckpoint?.Invoke(this, vehicle);
        }

        private bool CheckDirection(Transform other)
        {
            if (!OneWay)
                return true;

            var dir = (transform.position - other.position).normalized;
            var dot = Vector3.Dot(transform.forward, dir);

            return dot > 0f;
        }

    #if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if(!Trigger)
                return;
            
            var pos = Trigger.bounds.center;
            
            var cubeTransform = Matrix4x4.TRS(Trigger.transform.position, Trigger.transform.rotation, Trigger.transform.localScale);
            var oldGizmosMatrix = Gizmos.matrix;
     
            Gizmos.matrix *= cubeTransform;
            Gizmos.color = new Color(0f,1f,0f,0.2f);
            Gizmos.DrawCube(((BoxCollider)Trigger).center, ((BoxCollider)Trigger).size);
            Gizmos.matrix = oldGizmosMatrix;
            
            if(!OneWay)
                return;
            
            var direction = Trigger.transform.forward;
            var arrowHeadAngle = 45f;
            var arrowHeadLength = 0.25f;
            Gizmos.color = Color.green;
            Gizmos.DrawRay(pos, direction);
           
            var right = Quaternion.LookRotation(direction) * Quaternion.Euler(0,180+arrowHeadAngle,0) * new Vector3(0,0,1);
            var left = Quaternion.LookRotation(direction) * Quaternion.Euler(0,180-arrowHeadAngle,0) * new Vector3(0,0,1);
            Gizmos.DrawRay(pos + direction, right * arrowHeadLength);
            Gizmos.DrawRay(pos + direction, left * arrowHeadLength);
        }
        #endif
    }
}