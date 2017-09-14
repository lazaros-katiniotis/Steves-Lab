using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TogglableObject : MonoBehaviour, IParticleCollidable {

    public bool activeOnStart;

    protected bool activated;
    protected bool firstTimeActivated;

    private void Awake() {
        firstTimeActivated = false;
        activated = activeOnStart;
    }

    public abstract void Toggle(Actor actor);

    public abstract Component GetParticleColliderTransform();

    public bool IsActivated() {
        return activated;
    }

}
