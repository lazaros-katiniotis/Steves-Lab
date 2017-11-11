using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserEmitterScript : TogglableObject {

    public float startingAngle;

    private LaserScript laserBeam;

    public enum LaserEmitterStateEnum {
        TURN_ON, TURN_OFF, ROTATE, WAIT,
    }

    [Serializable]
    public class LaserEmitterState {
        public LaserEmitterStateEnum state;
        public LaserEmitterData data;
    }

    [Serializable]
    public class LaserEmitterData {
        public float targetAngle;
        public float duration;
    }

    public List<LaserEmitterState> states;

    private Queue<LaserEmitterState> queue;
    private LaserEmitterState currentState;

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

        if (activeOnStart) {
            laserBeam.Activate();
        } else {
            laserBeam.Deactivate();
        }

        queue = new Queue<LaserEmitterState>();
        for (int i = 0; i < states.Count; i++) {
            queue.Enqueue(states[i]);
        }

        PrepareNextState();
    }

    void Start() {

    }

    void Update() {
        //if (rotate) {
        //    laserBeam.LaserRotation(rotationSpeedMultiplier);
        //}
    }

    public override Component GetParticleColliderTransform() {
        throw new NotImplementedException();
    }

    public void PrepareNextState() {
        currentState = queue.Dequeue();
        laserBeam.SetCurrentState(currentState);
        queue.Enqueue(currentState);
    }
}
