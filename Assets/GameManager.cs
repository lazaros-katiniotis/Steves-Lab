using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public List<GameObject> machines;
    public PlayerController player;
    private static GameManager instance;

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

    public static GameManager GetInstance() {
        return instance;
    }
}
