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

    protected override void ToggleMachineFunction(PlayerController player) {
        if (doorScript.IsLocked()) {
            if (player.GetInventory().HasItem(PickupObjectScript.PickupObjectType.KEYCARD)) {
                player.GetInventory().RemoveItem(PickupObjectScript.PickupObjectType.KEYCARD);
                doorScript.Unlock();
            }
        } else {
            player.GetInventory().AddItem(PickupObjectScript.PickupObjectType.KEYCARD);
            doorScript.Lock();
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
