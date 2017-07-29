using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MachineScript : MonoBehaviour {

    public List<GameObject> affectedObjects;

    void Start() {

    }

    void Update() {

    }

    //public void ToggleMachineFunction() {
    //    Debug.Log("Machine function togled!");
    //    //foreach (GameObject obj in affectedObjects) {
    //    //    obj.SetActive(!obj.activeSelf);
    //    //}

    //    //StartCoroutine()

    //    StartCoroutine("")
    //}

    public abstract void ToggleMachineFunction();

}
