﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OxygenMachineScript : TerminalScript {

    protected override void ToggleTerminalFunction(Actor actor) {
        foreach (TogglableObject obj in affectedObjects) {
            AirVentScript vent = obj.GetComponent<AirVentScript>();
        }
    }

    public override void Toggle(Actor actor) {
        ToggleTerminalFunction(GameManager.GetInstance().player);
    }

    public override void TurnOn() {

    }

    public override void TurnOff() {

    }

    public override Component GetParticleColliderTransform() {
        throw new NotImplementedException();
    }
}
