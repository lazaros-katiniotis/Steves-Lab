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
        room = transform.GetComponentInParent<RoomScript>();
    }

    void Update() {

    }

    public override void Toggle() {
        activated = !activated;
        light.ToggleLight();
    }

    public override void TurnOn() {
        if (room == null) {
            room = transform.GetComponentInParent<RoomScript>();
        }
        activated = true;
        light.TurnOn();
        room.CalculateOxygenationPercentage();
    }

    public override void TurnOff() {
        if (room == null) {
            room = transform.GetComponentInParent<RoomScript>();
        }
        activated = false;
        light.TurnOff();
        room.CalculateOxygenationPercentage();
    }

    public override Component GetParticleColliderTransform() {
        throw new NotImplementedException();
    }
}
