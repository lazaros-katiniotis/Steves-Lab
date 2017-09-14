using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TogglableObject : MonoBehaviour, IParticleCollidable {

    public bool activeOnStart;

    protected bool activated;
    protected bool firstTimeActivated;
    protected bool interactable;

    private void Awake() {
        activated = activeOnStart;
    }

    void Start() {
        firstTimeActivated = false;
    }

    public abstract void Toggle(Actor actor);

    public abstract void TurnOn();

    public abstract void TurnOff();

    public abstract Component GetParticleColliderTransform();

    public bool IsActivated() {
        return activated;
    }

}
