using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineManager : MonoBehaviour {

    public List<GameObject> machines;
    private static MachineManager instance;

    void Start() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(this.gameObject);
            return;
        }
    }

    void Update() {

    }

    public static MachineManager GetInstance() {
        return instance;
    }
}
