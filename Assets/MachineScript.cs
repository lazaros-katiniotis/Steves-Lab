using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineScript : MonoBehaviour {

    public List<GameObject> affectedObjects;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}

    public void ToggleMachineFunction() {
        foreach (GameObject obj in affectedObjects) {
            obj.SetActive(!obj.activeSelf);
        }
    }
}
