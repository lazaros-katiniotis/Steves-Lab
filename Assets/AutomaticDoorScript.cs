using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticDoorScript : DoorScript {

    private void Awake() {
        Toggle(null);
        Close();
    }

    void Start() {

    }

    void Update() {
        if (currentPlayerInRange != prevPlayerInRange) {
            if (currentPlayerInRange) {
                Open();
            } else {
                Close();
            }
        }
        prevPlayerInRange = currentPlayerInRange;
    }

    public override void Toggle(Actor actor) {
        if (!activated) {
            if (actor != null) {
                if (actor.GetInventory().HasItem(PickupObjectScript.PickupObjectType.KEYCARD)) {
                    actor.GetInventory().RemoveItem(PickupObjectScript.PickupObjectType.KEYCARD);
                    activated = true;
                    OpenIndicator();
                    Open();
                }
            }
        } else {
            if (actor != null) {
                actor.GetInventory().AddItem(PickupObjectScript.PickupObjectType.KEYCARD);
                activated = false;
                CloseIndicator();
                Close();
            }
        }
    }

    public override Component GetParticleColliderTransform() {
        return null;
    }

    public override void Open() {
        if (activated) {
            closedSpriteTransform.gameObject.SetActive(false);
            openSpriteTransform.gameObject.SetActive(true);
        }
    }

    public override void Close() {
        closedSpriteTransform.gameObject.SetActive(true);
        openSpriteTransform.gameObject.SetActive(false);
    }
}
