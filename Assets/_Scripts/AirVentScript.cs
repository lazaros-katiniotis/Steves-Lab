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

    public bool IsActivated() {
        return activated;
    }

    public void ToggleVent() {
        activated = !activated;
        light.ToggleLight();
        room.CalculateOxygenationPercentage();
    }
}
