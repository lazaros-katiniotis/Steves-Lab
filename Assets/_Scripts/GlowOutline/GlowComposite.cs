using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class GlowComposite : MonoBehaviour {
    [Range(0, 10)]
    public float Intensity = 2;
    [Range(0, 1)]
    public float Cutoff = 0;

    //private float currentCutoff;
    //private float cutoffFactor;

    public Material compositeMaterial;

    void OnEnable() {
        //compositeMaterial = new Material(Shader.Find("Custom/GlowComposite"));
    }

    void OnRenderImage(RenderTexture src, RenderTexture dst) {
        compositeMaterial.SetFloat("_Intensity", Intensity);
        //compositeMaterial.SetFloat("_Cutoff", currentCutoff);
        Graphics.Blit(src, dst, compositeMaterial, 0);
    }

    private void Update() {
        //currentCutoff = Mathf.Lerp(currentCutoff, Cutoff, Time.deltaTime * cutoffFactor);
    }

    //public void StartGlow() {
    //    Cutoff = 1;
    //    cutoffFactor = 15;
    //}

    //public void EndGlow() {
    //    Cutoff = 1;
    //    cutoffFactor = 15;
    //}

    //public float GetCurrentCutoff() {
    //    return currentCutoff;
    //}
}
