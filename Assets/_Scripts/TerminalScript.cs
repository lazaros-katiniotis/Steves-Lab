using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerminalScript : TogglableObject, EnergyObject {

    public List<TogglableObject> affectedObjects;
    public List<LightningParticleScript> particleScriptList;
    public float duration = 30.0f;
    public Transform lightTransform;
    public GameObject lightningParticleSystem;

    private bool startTimer;
    private float elapsed;

    private int requiredEnergy = 1;

    private void Awake() {
        particleScriptList = new List<LightningParticleScript>();
        lightTransform.gameObject.SetActive(false);
        for (int i = 0; i < affectedObjects.Count; i++) {
            LightningParticleScript script = Instantiate(lightningParticleSystem, this.transform).GetComponent<LightningParticleScript>();
            script.Init(this, i);
            particleScriptList.Add(script);
        }
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
            if (GameManager.GetInstance().HasEnoughEnergy(requiredEnergy)) {
                activated = true;
                startTimer = true;
                elapsed = 0;
                ToggleAffectedObjects(actor);
                foreach (LightningParticleScript script in particleScriptList) {
                    script.gameObject.SetActive(true);
                }
            } else {
                //warning animation
            }
        } else {
            GameManager.GetInstance().IncreaseLevelEnergy(requiredEnergy);
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

    public int GetRequiredEnergy() {
        return requiredEnergy;
    }
}