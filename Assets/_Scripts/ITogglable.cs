using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITogglable {

    //public Transform colliderTransform;
    //public bool activeOnStart;

    //protected bool activated;
    //protected bool firstTimeActivated;
    //protected bool interactable;

    //private void Awake() {
    //    activated = activeOnStart;
    //}

    //void Start() {
    //    firstTimeActivated = false;
    //}

    void Toggle();

    void TurnOn();

    void TurnOff();

    bool IsActivated();

}
