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

    public override void ToggleMachineFunction(PlayerController player) {
        if (player.GetInventory().HasItem(PickupObjectScript.PickupObjectType.KEYCARD)) {
            player.GetInventory().RemoveItem(PickupObjectScript.PickupObjectType.KEYCARD);
            if (doorScript.IsLocked()) {
                doorScript.Unlock();
            }
        }
    }

}
