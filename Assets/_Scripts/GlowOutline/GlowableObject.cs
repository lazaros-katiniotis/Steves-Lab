using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowableObject : MonoBehaviour {

    public bool activate;
    public Color glowColor;
    public float lerpFactor;
    public Renderer[] renderers;
    [Range(0, 1)]
    public float cutoff;

    private List<Material> materials;
    private Color currentColor;
    private Color targetColor;

	void Start () {
        renderers = GetComponentsInChildren<Renderer>();
        materials = new List<Material>();
        foreach (Renderer renderer in renderers) {
            materials.AddRange(renderer.materials);
        }
	}

    private void OnMouseEnter() {
        targetColor = glowColor;
    }

    private void OnMouseExit() {
        targetColor = Color.black;
    }

    // Update is called once per frame
    void Update () {
        currentColor = Color.Lerp(currentColor, targetColor, Time.deltaTime * lerpFactor);

        for (int i = 0; i < materials.Count; i++) {
            materials[i].SetColor("_GlowColor", currentColor);
            materials[i].SetFloat("_Cutoff", cutoff);
        }
    }
}
