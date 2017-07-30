using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirVentScript : TogglableObject {

    public RoomScript room;
    private bool activated = false;
    private BlinkingLightScript light;

    void Start() {
        light = GetComponentInChildren<BlinkingLightScript>();
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
