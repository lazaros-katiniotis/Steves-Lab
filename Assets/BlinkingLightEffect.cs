using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkingLightEffect : MonoBehaviour {

    public float baseRange;
    public Color baseColor;
    public Color nextColor;

    private Light light;
    private float elapsed;
    private float colorElapsed;

    private float baseRangeOffset;
    private float rangeScaleOffset;
    private float phaseOffset;

    void Start() {
        light = GetComponent<Light>();
        baseRangeOffset = Random.Range(-0.1f, 0.1f);
        baseRangeOffset = -1.0f;
        rangeScaleOffset = Random.Range(0.4f, 0.6f);
        phaseOffset = Random.Range(0.25f, 0.5f);

        Debug.Log(transform.name + ": " + baseRangeOffset + ", " + rangeScaleOffset + ", " + phaseOffset);
    }

    void Update() {
        light.color = baseColor;
        elapsed += Time.deltaTime;
        colorElapsed += Time.deltaTime;

        light.range = baseRange + baseRangeOffset + rangeScaleOffset * Mathf.Sin(phaseOffset * Mathf.PI * elapsed / 2.0f);
        light.color = Color.Lerp(baseColor, nextColor, colorElapsed);

        if (colorElapsed >= 1.0f) {
            Color tempColor = baseColor;
            baseColor = nextColor;
            nextColor = tempColor;
            colorElapsed = 0.0f;
        }
    }
}
