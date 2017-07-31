using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelScript : MonoBehaviour {

    public Transform roomsTransform;

    // Use this for initialization
    void Start() {
        for (int i = 0; i < roomsTransform.childCount; i++) {
            RoomScript room = roomsTransform.GetChild(i).GetComponent<RoomScript>();
            for (int j = 0; j < room.GetDoors().childCount; j++) {
                GameManager.GetInstance().AddMachine(room.GetDoors().GetChild(j).GetComponent<DoorScript>());
            }
            for (int j = 0; j < room.GetMachines().childCount; j++) {
                GameManager.GetInstance().AddMachine(room.GetMachines().GetChild(j).GetComponent<MachineScript>());
            }
        }
    }

    // Update is called once per frame
    void Update() {

    }
}
