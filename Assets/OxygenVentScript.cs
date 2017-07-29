using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OxygenVentScript : TogglableObject {

    public RoomScript room;
    private bool activated = false;

    void Start() {

    }

    void Update() {

    }

    public bool IsActivated() {
        return activated;
    }

    public void ToggleVent() {
        activated = !activated;
        room.CalculateOxygenationPercentage();
    }
}
