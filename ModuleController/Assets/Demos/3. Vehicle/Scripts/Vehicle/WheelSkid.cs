using System;
using System.Collections;
using System.Collections.Generic;
using Demos.Vehicle;
using UnityEngine;

public class WheelSkid : MonoBehaviour
{
    public CustomWheel Wheel;
    public TrailRenderer Trail;
    [Range(0f, 1f)]
    public float Threshold = 0.2f;


    private void Update()
    {
        
        var skid = 0f;

        if (Wheel.GroundData.HasGround)
        {
            skid = Mathf.Abs(Wheel.GroundData.Hit.sidewaysSlip);
            Trail.transform.position = Wheel.GroundData.Hit.point + Wheel.GroundData.Hit.normal * 0.05f;
        }

        Trail.emitting = skid > Threshold;
    }
}
