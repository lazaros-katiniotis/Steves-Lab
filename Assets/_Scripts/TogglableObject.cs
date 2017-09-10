using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TogglableObject : MonoBehaviour, IParticleCollidable {

    public Transform colliderTransform;
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

    public abstract void Toggle();

    protected abstract void TurnOn();

    protected abstract void TurnOff();

    public bool IsActivated() {
        return activated;
    }

    public Component GetColliderTransform() {
        return colliderTransform;
    }

}
