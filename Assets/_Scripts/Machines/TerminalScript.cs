using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TerminalScript : TogglableObject {

    public List<TogglableObject> affectedObjects;
    private float elapsed;
    public float duration = 30.0f;

    private void OnEnable() {
        elapsed = 0.0f;
    }

    void Start() {
        activated = false;
    }

    protected void Update() {
        if (activated) {
            elapsed += Time.deltaTime;
            if (elapsed > duration) {
                elapsed -= duration;
                if (activated) {
                    ToggleTerminalFunction(null);
                }
                activated = false;
            }
        }
    }

    protected abstract void ToggleTerminalFunction(PlayerController player);

}
