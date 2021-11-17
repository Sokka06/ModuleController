using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reset : MonoBehaviour
{
    public Vector3 Position;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other is CharacterController characterController)
        {
            characterController.enabled = false;
            other.transform.position = Position;
            characterController.enabled = true;
        }
    }
}
