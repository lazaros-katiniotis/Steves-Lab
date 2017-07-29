using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorMachineScript : MachineScript {

    private DoorScript doorScript;

    // Use this for initialization
    void Start() {
        doorScript = GetComponentInParent<DoorScript>();
    }

    // Update is called once per frame
    void Update() {

    }

    public override void ToggleMachineFunction() {
        Debug.Log("Will unlock door after removing a keycard.");
        if (doorScript.IsLocked()) {
            doorScript.Unlock();
        }
    }

}
