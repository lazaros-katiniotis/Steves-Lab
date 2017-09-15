using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerminalScript : TogglableObject {

    public List<TogglableObject> affectedObjects;
    public float duration = 30.0f;
    public Transform lightTransform;

    private bool startTimer;
    private float elapsed;

    private void Awake() {
        lightTransform.gameObject.SetActive(false);
    }

    private void Start() {
        startTimer = activeOnStart;
        elapsed = 0.0f;
        if (activeOnStart) {
            activated = false; //hack
            Toggle(null);
        }
    }

    protected void Update() {
        if (startTimer) {
            elapsed += Time.deltaTime;
            if (elapsed >= duration) {
                startTimer = false;
                Toggle(null);
            }
        }
    }

    public override void Toggle(Actor actor) {
        if (!activated) {
            activated = true;
            startTimer = true;
            elapsed = 0;
            ToggleAffectedObjects(actor);
        } else {
            activated = false;
            startTimer = false;
            ToggleAffectedObjects(actor);
        }
        lightTransform.gameObject.SetActive(activated);
    }

    public void ToggleAffectedObjects(Actor actor) {
        foreach (TogglableObject obj in affectedObjects) {
            obj.Toggle(actor);
        }
    }

    public override Component GetParticleColliderTransform() {
        throw new NotImplementedException();
    }
}
