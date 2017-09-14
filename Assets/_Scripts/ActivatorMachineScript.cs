using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatorMachineScript : TerminalScript {

    private Light light;
    public GameObject lightningParticleSystem;

    void Start() {
        light = GetComponentInChildren<Light>();
    }

    void Update() {
        base.Update();
        light.enabled = activated;
    }

    protected override void ToggleTerminalFunction(Actor actor) {
        activated = !activated;
        lightningParticleSystem.SetActive(activated);
        foreach (TogglableObject obj in affectedObjects) {
            Debug.Log("Toggling: " + obj.transform.name);
            obj.Toggle(actor);
        }
    }

    public override void Toggle(Actor actor) {
        if (firstTimeActivated) {
            TurnOn();
            firstTimeActivated = false;
        } else {
            ToggleTerminalFunction(actor);
        }
    }

    public void TurnOn() {
        throw new NotImplementedException();
    }

    public void TurnOff() {
        throw new NotImplementedException();
    }

    //protected override void TurnOn() {
    //    activated = true;
    //    foreach (TogglableObject obj in affectedObjects) {
    //        Debug.Log(obj.transform.name);
    //        if (obj.GetType().Equals(typeof(DoorScript))) {
    //            ((DoorScript)obj).Unlock();
    //        }
    //        obj.TurnOn();
    //    }
    //}

    //protected override void TurnOff() {
    //    activated = false;
    //    foreach (TogglableObject obj in affectedObjects) {
    //        obj.TurnOff();
    //    }
    //}

    public override Component GetParticleColliderTransform() {
        throw new NotImplementedException();
    }
}
