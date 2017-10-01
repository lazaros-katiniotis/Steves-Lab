using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowableObject : MonoBehaviour {

    public bool activate;
    public Color glowColor;
    public float lerpFactor;
    public Renderer[] renderers;
    [Range(0, 1)]
    public float targetCutoff;
    private float currentCutoff;
    private float cutoffFactor = 1;
    private float cutoffElapsed = 0;

    private List<Material> materials;
    private Color currentColor;
    private Color targetColor;

    private GlowComposite glowCompositeScript;

    void Start() {
        renderers = GetComponentsInChildren<Renderer>(true);
        materials = new List<Material>();
        glowCompositeScript = Camera.main.GetComponent<GlowComposite>();
        foreach (Renderer renderer in renderers) {
            materials.AddRange(renderer.materials);
        }
    }

    public void StartGlow() {
        targetColor = glowColor;
        targetCutoff = 1.0f;
        //currentCutoff = 1.0f;
        cutoffFactor = 5;
        cutoffElapsed = 0.0f;
    }

    public void EndGlow() {
        //targetColor = Color.black;
        targetColor = glowColor;
        targetCutoff = 0.0f;
        cutoffFactor = 7.5f;
        cutoffElapsed = 0.0f;
    }

    void Update() {
        cutoffElapsed += Time.deltaTime * cutoffFactor;
        //currentColor = Color.Lerp(currentColor, targetColor, cutoffElapsed);
        //currentColor = targetColor;
        currentColor = glowColor;
        currentCutoff = Mathf.Lerp(currentCutoff, targetCutoff, cutoffElapsed);
        for (int i = 0; i < materials.Count; i++) {
            materials[i].SetColor("_GlowColor", currentColor);
            materials[i].SetFloat("_Cutoff", currentCutoff);
        }
    }

    //private IEnumerator ResetCutoff() {
    //    while (targetCutoff < 0.01f) {
    //        targetCutoff = glowCompositeScript.GetCurrentCutoff();
    //        yield return null;
    //    }
    //    targetCutoff = 0.0f;
    //    yield return null;
    //}
}
