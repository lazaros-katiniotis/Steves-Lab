using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OxygenMachineScript : MachineScript {

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    protected override void ToggleMachineFunction(PlayerController player) {
        foreach (TogglableObject obj in affectedObjects) {
            AirVentScript vent = obj.GetComponent<AirVentScript>();

        }
    }

    public override void Toggle() {
        ToggleMachineFunction(GameManager.GetInstance().player);
    }

    public override void TurnOn() {

    }

    public override void TurnOff() {

    }
}
