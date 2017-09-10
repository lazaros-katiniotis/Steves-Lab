﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OxygenMachineScript : TerminalScript {

    protected override void ToggleTerminalFunction(PlayerController player) {
        foreach (TogglableObject obj in affectedObjects) {
            AirVentScript vent = obj.GetComponent<AirVentScript>();

        }
    }

    public override void Toggle() {
        ToggleTerminalFunction(GameManager.GetInstance().player);
    }

    protected override void TurnOn() {

    }

    protected override void TurnOff() {

    }
}
