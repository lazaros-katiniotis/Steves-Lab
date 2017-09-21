using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserEmitterScript : MonoBehaviour {

    public bool rotate;
    public float rotationSpeedMultiplier;

    private LaserScript laserBeam;

    private void Awake() {
        laserBeam = GetComponentInChildren<LaserScript>();
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
}
