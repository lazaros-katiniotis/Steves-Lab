using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewDoorScript : TogglableObject {

    public Transform closedSpriteTransform;
    public Transform openSpriteTransform;

    public override void Toggle() {

    }

    public override void TurnOn() {
        closedSpriteTransform.gameObject.SetActive(false);
        openSpriteTransform.gameObject.SetActive(true);
    }

    public override void TurnOff() {
        closedSpriteTransform.gameObject.SetActive(true);
        openSpriteTransform.gameObject.SetActive(false);
    }
}
