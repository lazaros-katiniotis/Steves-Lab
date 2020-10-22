using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkingLightScript : MonoBehaviour {

    public Color baseColor;
    public Color nextColor;

    private Light light;
    private float elapsed;
    private float colorElapsed;

    public Vector2 baseRangeOffset;
    public float rangeScaleOffset = 1.0f;
    public float phaseOffset = 1.0f;

    private float baseRange;
    private float offset;

    [Range(1.0f, 3.0f)]
    public float blinkSpeed;

    void Start() {
        light = GetComponent<Light>();
        baseRange = light.range;
        offset = UnityEngine.Random.Range(baseRangeOffset.x, baseRangeOffset.y);
        elapsed = UnityEngine.Random.Range(0.0f, 1.0f);
        phaseOffset = UnityEngine.Random.Range(0.9f, 1.1f);
        //baseRangeOffset = -1.0f;
        //rangeScaleOffset = Random.Range(0.4f, 0.6f);
        //phaseOffset = Random.Range(0.25f, 0.5f);

        //Debug.Log(transform.name + ": " + baseRangeOffset + ", " + rangeScaleOffset + ", " + phaseOffset);
    }

    void Update() {
        light.color = baseColor;
        elapsed += Time.deltaTime;
        colorElapsed += Time.deltaTime;

        light.range = (baseRange + offset) + rangeScaleOffset * Mathf.Sin(phaseOffset * Mathf.PI * elapsed / blinkSpeed);
        light.color = Color.Lerp(baseColor, nextColor, colorElapsed);

        if (colorElapsed >= 1.0f) {
            Color tempColor = baseColor;
            baseColor = nextColor;
            nextColor = tempColor;
            colorElapsed = 0.0f;
        }
    }

    public void ToggleLight() {
        light.enabled = !light.enabled;
    }

    public void TurnOff() {
        if (light == null) {
            light = GetComponent<Light>();
        }
        light.enabled = false;
    }

    public void TurnOn() {
        if (light == null) {
            light = GetComponent<Light>();
        }
        light.enabled = true;
    }
}
