using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Actor : MonoBehaviour {

    public abstract InventoryManager GetInventory();

    public abstract void UpdateDraggingState(bool isDragging, TogglableObject obj);

}
