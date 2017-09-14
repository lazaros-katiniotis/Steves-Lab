using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorMachineScript : TerminalScript {

    private OldDoorScript doorScript;

    // Use this for initialization
    void Start() {
        doorScript = GetComponentInParent<OldDoorScript>();
    }

    // Update is called once per frame
    void Update() {

    }

    protected override void ToggleTerminalFunction(Actor actor) {
        PlayerController player = (PlayerController)actor;
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

    public override void Toggle(Actor actor) {
        ToggleTerminalFunction(actor);
    }

    public override void TurnOn() {

    }

    public override void TurnOff() {

    }

    public override Component GetParticleColliderTransform() {
        throw new NotImplementedException();
    }
}
