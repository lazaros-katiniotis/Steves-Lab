using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallLampScript : TogglableObject {

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

    public override void TurnOff() {
        light.TurnOff();
        activated = false;
    }

    public override void TurnOn() {
        light.TurnOn();
        activated = true;
    }

    public override Component GetParticleColliderTransform() {
        throw new NotImplementedException();
    }
}
