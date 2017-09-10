using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomScript : MonoBehaviour {

    private bool roomOxygenated = false;
    private float oxygenLevel;
    private float oxygenMax;
    private float oxygenMultiplier;
    private int floorCount;

    public Transform floorTilesTransform;
    public Transform airVentsTransform;
    public Transform terminalsTransform;
    public Transform doorsTransform;

    private List<AirVentScript> oxygenVents;

    private float oxygenationPercentage;

    // Use this for initialization
    void Start() {
        floorCount = floorTilesTransform.childCount;
        oxygenMultiplier = 1.0f;
        oxygenMax = oxygenMultiplier * floorCount;
        oxygenLevel = oxygenMax;
        oxygenVents = new List<AirVentScript>();

        for (int i = 0; i < airVentsTransform.childCount; i++) {
            oxygenVents.Add(airVentsTransform.GetChild(i).GetComponent<AirVentScript>());
        }

        CalculateOxygenationPercentage();
    }

    private float elapsed;

    // Update is called once per frame
    void Update() {
        if (oxygenationPercentage == 1.0f) {
            oxygenLevel += Time.deltaTime * 5f;

        } else {
            //oxygenLevel -= Time.deltaTime * (1 - oxygenationPercentage);
            oxygenLevel = 0.0f;
        }

        //elapsed += Time.deltaTime;
        //if (elapsed > 1.0f) {
        //    Debug.Log("OxygenationPercentage: " + oxygenationPercentage + ", oxygen Level: " + oxygenLevel);
        //    elapsed -= 1.0f;
        //}

        if (oxygenLevel > oxygenMax) {
            oxygenLevel = oxygenMax;
        } else if (oxygenLevel < 0.0f) {
            oxygenLevel = 0.0f;
        }
    }

    public float GetCurrentOxygenPercent() {
        return (oxygenLevel / oxygenMax);
    }

    public bool IsRoomOxygenated() {


        return false;
    }

    public void CalculateOxygenationPercentage() {
        int activatedVents = 0;
        float totalVents = 0;

        if (oxygenVents != null) {
            totalVents = oxygenVents.Count;
        }

        if (totalVents == 0) {
            oxygenationPercentage = 0;
            return;
        }

        foreach (AirVentScript vent in oxygenVents) {
            if (vent.IsActivated()) {
                activatedVents++;
            }
        }

        //Debug.Log("Oxygenation Percentage: " + activatedVents / totalVents);
        oxygenationPercentage = (activatedVents / totalVents);
    }

    public Transform GetDoors() {
        return doorsTransform;
    }

    public Transform GetTerminals() {
        return terminalsTransform;
    }
}
