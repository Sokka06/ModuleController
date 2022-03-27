using System;
using System.Collections;
using System.Collections.Generic;
using Demos.Vehicle;
using UnityEngine;

public class WheelSkid : MonoBehaviour
{
    public CustomWheel Wheel;
    public AnimationCurve SkidCurve = AnimationCurve.EaseInOut(0f,0f,1f,1f);
    public TrailRenderer SkidTrail;

    //private Skidmarks _skidmarks;
    private int _skidIndex = -1;

    private void Start()
    {
        //_skidmarks = Skidmarks.Instance;
    }

    private void LateUpdate()
    {
        /*var skid = 0f;

        if (Wheel.GroundData.HasGround)
        {
            skid = Mathf.Abs(Wheel.GroundData.Hit.sidewaysSlip);
            _skidIndex = _skidmarks.AddSkidMark(
                Wheel.GroundData.Hit.point + Wheel.GroundData.Hit.normal * 0.05f, Wheel.GroundData.Hit.normal, SkidCurve.Evaluate(skid),
                _skidIndex);
            SkidTrail.transform.position = Wheel.GroundData.Hit.point + Wheel.GroundData.Hit.normal * 0.05f;
        }
        else {
            _skidIndex = -1;
        }

        SkidTrail.emitting = skid > 0.1f;*/
    }
}
