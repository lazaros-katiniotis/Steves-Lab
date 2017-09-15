using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechanicalDoorScript : DoorScript {

    public Transform particleColliderTransform;
    private MechanicalDoorInteractionScript doorInteractionScript;

    private void Awake() {
        doorInteractionScript = GetComponentInChildren<MechanicalDoorInteractionScript>();
    }

    public override void Toggle(Actor actor) {
        if (!activated) {
            activated = true;
            OpenIndicator();
            Open();
        } else {
            activated = false;
        }
    }

    void Start() {

    }

    void Update() {
        if (!activated) {
            if (!currentPlayerInRange) {
                CloseIndicator();
                Close();
            }
        }
        prevPlayerInRange = currentPlayerInRange;
    }

    public override void Open() {
        //if (activated) {
        closedSpriteTransform.gameObject.SetActive(false);
        openSpriteTransform.gameObject.SetActive(true);
        //}
    }

    public override void Close() {
        closedSpriteTransform.gameObject.SetActive(true);
        openSpriteTransform.gameObject.SetActive(false);
    }

    public override Component GetParticleColliderTransform() {
        return particleColliderTransform;
    }

}
