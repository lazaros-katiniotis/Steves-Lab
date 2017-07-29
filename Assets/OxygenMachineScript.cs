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

    public override void ToggleMachineFunction() {
        foreach (TogglableObject obj in affectedObjects) {
            OxygenVentScript vent = obj.GetComponent<OxygenVentScript>();

        }
    }
}
