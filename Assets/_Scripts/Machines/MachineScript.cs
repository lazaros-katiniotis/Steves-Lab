using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MachineScript : TogglableObject {

    public List<TogglableObject> affectedObjects;
    private float elapsed;
    public float duration = 30.0f;

    void Start() {
        activated = false;
    }

    protected void Update() {
        if (activated) {
            elapsed += Time.deltaTime;
            if (elapsed > duration) {
                elapsed -= duration;
                if (activated) {
                    ToggleMachineFunction(null);
                }
                activated = false;
            }
        }
    }

    protected abstract void ToggleMachineFunction(PlayerController player);

}
