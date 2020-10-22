using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxPuzzle : MonoBehaviour {

    public Transform floorTiles;

    private bool active;
    private bool prevActive;

    public List<TogglableObject> affectedObjects;

    void Start() {
        active = false;
        prevActive = false;
    }

    void Update() {
        active = AreAllFloorTilesActive();
        if (active != prevActive) {
            foreach (TogglableObject obj in affectedObjects) {
                obj.Toggle(null);
            }
        }
        prevActive = active;
    }

    private bool AreAllFloorTilesActive() {
        for (int i = 0; i < floorTiles.childCount; i++) {
            ButtonFloor floor = floorTiles.GetChild(i).GetComponent<ButtonFloor>();
            if (!floor.IsActive()) {
                return false;
            }
        }
        return true;
    }
}
