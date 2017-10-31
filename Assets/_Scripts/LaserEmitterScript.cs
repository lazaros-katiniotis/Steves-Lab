using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserEmitterScript : TogglableObject {

    public bool rotate;
    public float startingAngle;
    public float rotationSpeedMultiplier;

    private LaserScript laserBeam;

    public override void Toggle(Actor actor) {
        //if (!activated) {
        //    activated = true;
        //} else {
        //    activated = false;
        //}
    }

    private void Awake() {
        laserBeam = GetComponentInChildren<LaserScript>();
        laserBeam.SetRotation(startingAngle);
    }
    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (rotate) {
            laserBeam.LaserRotation(rotationSpeedMultiplier);
        }
    }

    public override Component GetParticleColliderTransform() {
        throw new NotImplementedException();
    }
}
