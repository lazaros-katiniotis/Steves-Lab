using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewDoorScript : TogglableObject {

    public Transform closedSpriteTransform;
    public Transform openSpriteTransform;

    private bool locked;

    private void Awake() {
        locked = true;
    }

    public override void Toggle() {

    }

    public override void TurnOn() {
        if (!locked) {
            closedSpriteTransform.gameObject.SetActive(false);
            openSpriteTransform.gameObject.SetActive(true);
        }
    }

    public override void TurnOff() {
        closedSpriteTransform.gameObject.SetActive(true);
        openSpriteTransform.gameObject.SetActive(false);
    }

    public override Component GetParticleColliderTransform() {
        throw new NotImplementedException();
    }
}
