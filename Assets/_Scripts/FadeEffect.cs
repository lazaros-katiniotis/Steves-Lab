using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class FadeEffect : MonoBehaviour {

    public Material effectMaterial;

    private float fadeElapsed;
    public float fadeSlow;
    private bool startFade = false;

    private void Awake() {
        FadeIn();
        StartCoroutine(DelayFadeUpdate(1.0f));
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination) {
        Graphics.Blit(source, destination, effectMaterial);
    }

    public void Update() {
        if (startFade) {
            fadeElapsed += Time.deltaTime / fadeSlow;
        }
        effectMaterial.SetFloat("_elapsed", fadeElapsed);
    }

    public void FadeIn() {
        fadeElapsed = 0.0f;
        effectMaterial.SetFloat("_FadeIn", 1.0f);
    }

    public void FadeOut() {
        fadeElapsed = 0.0f;
        effectMaterial.SetFloat("_FadeIn", 0.0f);
    }

    public IEnumerator DelayFadeUpdate(float seconds) {
        float elapsed = 0.0f;
        while (elapsed < seconds) {
            elapsed += Time.deltaTime;
            yield return null;
        }
        startFade = true;
        yield return null;
    }
}
