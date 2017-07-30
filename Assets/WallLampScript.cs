using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallLampScript : TogglableObject {

    private BlinkingLightScript light;
    private bool activated;

    void Start() {
        light = GetComponentInChildren<BlinkingLightScript>();
        activated = false;
    }

    void Update() {

    }

    public void ToggleLamp() {
        activated = !activated;
        light.ToggleLight();
    }
}
