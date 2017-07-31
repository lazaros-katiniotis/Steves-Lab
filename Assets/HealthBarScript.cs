using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarScript : MonoBehaviour {

    private float totalPercentage;
    private Material material;

    // Use this for initialization
    void Start() {
        totalPercentage = 1.0f;
        material = GetComponent<UnityEngine.UI.Image>().material;

        material.SetFloat("_Cutoff", totalPercentage);
    }

    // Update is called once per frame
    void Update() {

    }

    public void SetValue(float value) {
        material.SetFloat("_Cutoff", value);
    }

    public void RemoveChunk(float percentage) {

    }
}
