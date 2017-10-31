using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Actor : MonoBehaviour {

    public RoomScript currentRoom;
    public float speed = 4;
    public bool dragging;

    public abstract InventoryManager GetInventory();

    public abstract void UpdateDraggingState(bool isDragging, Vector2 relativePosition, BoxScript obj);

    public void SetCurrentRoom(RoomScript room) {
        currentRoom = room;
    }

    public RoomScript GetCurrentRoom() {
        return currentRoom;
    }

    public bool IsDragging() {
        return dragging;
    }

    public void SetDragging(bool value) {
        this.dragging = value;
    }
    
}
