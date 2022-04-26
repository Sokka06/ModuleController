using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Demos.Vehicle
{
    /// <summary>
    /// Holds all checkpoints.
    /// </summary>
    public class CheckpointManager : MonoBehaviour
    {
        public List<Checkpoint> Checkpoints;
        public string NamePrefix = "Checkpoint";
        
        public void FindCheckpoints()
        {
            Checkpoints = new List<Checkpoint>(GetComponentsInChildren<Checkpoint>());
        }
        
        public void SetupCheckpoints()
        {
            
        }
        
        public void RenameCheckpoints()
        {
            for (int i = 0; i < Checkpoints.Count; i++)
            {
                Checkpoints[i].name = $"{NamePrefix} {i}";
            }
        }

        public void AddCheckpoint(Checkpoint checkpoint)
        {
            Checkpoints.Add(checkpoint);
        }
        
        public void RemoveCheckpoint(Checkpoint checkpoint)
        {
            Checkpoints.Remove(checkpoint);
        }
        
        public void RemoveCheckpoint(int index)
        {
            Checkpoints.RemoveAt(index);
        }
        
        /// <summary>
        /// Finds checkpoint for given index.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="checkpoint"></param>
        /// <returns>true if checkpoint was found, false if not</returns>
        public bool GetCheckpoint(int index, out Checkpoint checkpoint)
        {
            checkpoint = Checkpoints[index];
            return checkpoint != null;
        }
        
        /// <summary>
        /// Gets index of given checkpoint.
        /// </summary>
        /// <param name="checkpoint"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool GetIndex(Checkpoint checkpoint, out int index)
        {
            index = Checkpoints.IndexOf(checkpoint);
            return index != -1;
        }
        
        /// <summary>
        /// Gets next index for given index.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="loop"></param>
        /// <returns></returns>
        public int GetNextIndex(int index, bool loop = true)
        {
            var lastIndex = GetLastIndex();
            var prevIndex = index + 1;
            if (loop && prevIndex > lastIndex)
                prevIndex = 0;
            
            return Mathf.Clamp(prevIndex, 0 , lastIndex);
        }
        
        /// <summary>
        /// gets previous index for given index
        /// </summary>
        /// <param name="index"></param>
        /// <param name="loop"></param>
        /// <returns></returns>
        public int GetPreviousIndex(int index, bool loop = true)
        {
            var lastIndex = GetLastIndex();
            var prevIndex = index - 1;
            if (loop && prevIndex < 0)
                prevIndex = lastIndex;
            
            return Mathf.Clamp(prevIndex, 0 , lastIndex);
        }

        /// <summary>
        /// Gets last checkpoint index
        /// </summary>
        /// <returns></returns>
        public int GetLastIndex()
        {
            return Checkpoints.Count - 1;
        }

        /// <summary>
        /// Is given checkpoint index last index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool IsLast(int index)
        {
            return index == GetLastIndex();
        }
        
    #if UNITY_EDITOR
        [ContextMenu("Update Checkpoints")]
        public void UpdateWaypoints()
        {
            FindCheckpoints();
            SetupCheckpoints();
            RenameCheckpoints();
        }

        private void OnDrawGizmos()
        {
            for (int i = 0; i < Checkpoints.Count; i++)
            {
                var tempColor = Color.green;
                tempColor.a *= 0.5f;
                Gizmos.color = tempColor;
                
                Handles.Label(Checkpoints[i].Trigger.bounds.center, "Checkpoint " + i);
                
                /*Gizmos.DrawSphere(Checkpoints[i].transform.position, 0.1f);
                
                if (i == 0)
                    continue;

                var prevIndex = i - 1;
                
                if (prevIndex < 0)
                    continue;
                
                Gizmos.DrawLine(Checkpoints[prevIndex].transform.position, Checkpoints[i].transform.position);*/
            }
        }
        
    #endif
    }
}

