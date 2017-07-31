using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirVentScript : TogglableObject {

    public RoomScript room;

    private BlinkingLightScript light;

    void Start() {
        light = GetComponentInChildren<BlinkingLightScript>();
        TurnOff();
        if (activeOnStart) {
            TurnOn();
        }
    }

    void Update() {

    }

    public override void Toggle() {
        activated = !activated;
        light.ToggleLight();
    }

    public override void TurnOn() {
        activated = true;
        light.TurnOn();
        room.CalculateOxygenationPercentage();
    }

    public override void TurnOff() {
        activated = false;
        light.TurnOff();
        room.CalculateOxygenationPercentage();
    }
}
