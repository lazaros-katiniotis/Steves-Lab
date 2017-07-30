using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallLampScript : TogglableObject {

    private BlinkingLightScript light;
    private bool activated;

    void Start() {
        light = GetComponentInChildren<BlinkingLightScript>();
        TurnOffLamp();
        if (activeOnStart) {
            TurnOnLamp();
        }
    }

    void Update() {

    }

    public void ToggleLamp() {
        activated = !activated;
        light.ToggleLight();
    }

    public void TurnOffLamp() {
        light.TurnOff();
        activated = false;
    }

    public void TurnOnLamp() {
        light.TurnOn();
        activated = true;
    }
}
