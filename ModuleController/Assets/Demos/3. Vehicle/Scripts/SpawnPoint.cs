using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public void GetSpawn(out Vector3 position, out Quaternion rotation)
    {
        position = transform.position;
        rotation = transform.rotation;
    }

    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        var color = Color.cyan;
        color.a *= 0.5f;
        Gizmos.color = color;
        
        Gizmos.DrawCube(transform.position, transform.lossyScale);
    }
    #endif
}
