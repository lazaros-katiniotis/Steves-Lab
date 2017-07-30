using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TogglableObject : MonoBehaviour {

    public bool activeOnStart;
    public bool activated;

    void Start () {

	}

    void Update() {

    }

    public abstract void Toggle();

    public abstract void TurnOn();

    public abstract void TurnOff();

    public bool IsActivated() {
        return activated;
    }
}
