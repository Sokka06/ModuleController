using System;
using System.Collections;
using System.Collections.Generic;
using Demos.Vehicle;
using UnityEngine;

public class WheelSkid : MonoBehaviour
{
    public CustomWheel Wheel;
    public TrailRenderer Trail;
    public ParticleSystem Smoke;
    [Range(0f, 1f)]
    public float Threshold = 0.2f;

    private void Update()
    {
        var skid = 0f;

        if (Wheel.GroundData.HasGround)
        {
            skid = Mathf.Abs(Wheel.SlipAngle) / 90f;
            var position = Wheel.GroundData.Hit.point + Wheel.GroundData.Hit.normal * 0.05f;
            Trail.transform.position = position;
            Smoke.transform.position = position;
        }

        if (skid > Threshold)
        {
            Trail.emitting = true;
            if (!Smoke.isEmitting)
                Smoke.Play();
        }
        else
        {
            Trail.emitting = false;
            
            if (Smoke.isPlaying)
                Smoke.Stop();
        }
    }
}
