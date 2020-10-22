using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DoorScript : TogglableObject {

    public Transform closedSpriteTransform;
    public Transform openSpriteTransform;
    public Transform closedIndicatorTransform;
    public Transform openIndicatorTransform;

    public RoomScript previousRoom;
    public RoomScript nextRoom;

    protected bool currentPlayerInRange;
    protected bool prevPlayerInRange;

    public abstract void Open();

    public abstract void Close();

    public void OpenIndicator() {
        if (closedIndicatorTransform != null) {
            closedIndicatorTransform.gameObject.SetActive(false);
        }
        if (openIndicatorTransform != null) {
            openIndicatorTransform.gameObject.SetActive(true);
        }
    }

    public void CloseIndicator() {
        if (closedIndicatorTransform != null) {
            closedIndicatorTransform.gameObject.SetActive(true);
        }
        if (openIndicatorTransform != null) {
            openIndicatorTransform.gameObject.SetActive(false);
        }
    }

    public RoomScript GetNextRoom() {
        return nextRoom;
    }

    public RoomScript GetPreviousRoom() {
        return previousRoom;
    }

    public void SetPlayerInRange(bool value) {
        currentPlayerInRange = value;
    }
}
