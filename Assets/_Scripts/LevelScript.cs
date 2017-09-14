using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelScript : MonoBehaviour {

    public Transform roomsTransform;

    void Start() {
        //for (int i = 0; i < roomsTransform.childCount; i++) {
        //    RoomScript room = roomsTransform.GetChild(i).GetComponent<RoomScript>();
        //    for (int j = 0; j < room.GetDoors().childCount; j++) {
        //        GameManager.GetInstance().AddTerminal(room.GetDoors().GetChild(j).GetComponent<DoorScript>());
        //    }
        //    for (int j = 0; j < room.GetTerminals().childCount; j++) {
        //        GameManager.GetInstance().AddTerminal(room.GetTerminals().GetChild(j).GetComponent<TerminalScript>());
        //    }
        //}
    }

}
