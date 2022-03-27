using System;
using System.Collections;
using System.Collections.Generic;
using Demos.Vehicle;
using TMPro;
using UnityEngine;

public class GearUI : MonoBehaviour
{
    public TextMeshProUGUI GearText;

    private VehicleGearboxModule _gearboxModule;

    private void Start()
    {
        var vehicle = FindObjectOfType<DriverManager>().CurrentDrivers[0].Vehicle;
        _gearboxModule = vehicle.Controller.GetModule<VehicleGearboxModule>();
        _gearboxModule.Gearbox.onGearChanged += OnGearChanged;
        
        GearText.SetText(GearToText(_gearboxModule.Gearbox.CurrentGearIndex));
    }

    private void OnDestroy()
    {
        _gearboxModule.Gearbox.onGearChanged -= OnGearChanged;
    }

    private void OnGearChanged(int gear)
    {
        GearText.SetText(GearToText(gear));
    }
    
    private string GearToText(int gear)
    {
        switch (gear)
        {
            case 0:
                return "R";
            case 1:
                return "N";
            case 2:
                return "D";
        }

        return "";
    }
}
