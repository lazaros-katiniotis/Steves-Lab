using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MachineScript : MonoBehaviour {

    public List<TogglableObject> affectedObjects;
    public bool activated = false;
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

    public abstract void ToggleMachineFunction(PlayerController player);

}
