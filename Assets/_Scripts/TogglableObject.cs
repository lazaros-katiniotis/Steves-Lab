using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TogglableObject : MonoBehaviour {

    public bool activeOnStart;
    protected bool activated;
    protected bool firstTimeActivated;

    void Start() {
        firstTimeActivated = false;
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
