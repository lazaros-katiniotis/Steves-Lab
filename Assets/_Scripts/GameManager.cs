using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public List<GameObject> machines;
    public PlayerController player;
    public Transform currentLevelTransform;
    public Transform canvasTransform;
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

    public void GameOver(Camera mainCamera) {
        this.transform.SetParent(null);
        mainCamera.transform.SetParent(this.transform);
        mainCamera.enabled = true;
        currentLevelTransform.gameObject.SetActive(false);
        canvasTransform.DetachChildren();
    }

    public static GameManager GetInstance() {
        return instance;
    }
}
