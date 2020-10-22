using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GlowPrePass : MonoBehaviour {

    private RenderTexture prePass;
    private RenderTexture blurred;

    private int downscaleFactor = 0;

    private string _prepassTexture = "_GlowPrePassTex";
    private string _blurTexture = "_GlowBlurTex";

    private Material blurMaterial;

    private void OnEnable() {
        Camera camera = GetComponent<Camera>();
        Shader glowShader = Shader.Find("Custom/GlowReplace");
        if (camera.targetTexture != null) {
            RenderTexture temp = camera.targetTexture;
            camera.targetTexture = null;
            DestroyImmediate(temp);
        }

        prePass = CreateRenderTexture(camera.pixelWidth, camera.pixelHeight, 0, 24, FilterMode.Point);
        prePass.antiAliasing = 1;
        blurred = CreateRenderTexture(camera.pixelWidth, camera.pixelHeight, 1, 24, FilterMode.Bilinear);

        camera.targetTexture = prePass;
        camera.SetReplacementShader(glowShader, "Glowable");
        Shader.SetGlobalTexture(_prepassTexture, prePass);
        Shader.SetGlobalTexture(_blurTexture, blurred);

        blurMaterial = new Material(Shader.Find("Blurs/Blur"));
        blurMaterial.SetVector("_BlurSize", new Vector2(blurred.texelSize.x, blurred.texelSize.y));
    }

    //Blur pass
    void OnRenderImage(RenderTexture src, RenderTexture dst) {
        Graphics.Blit(src, dst);

        Graphics.SetRenderTarget(blurred);
        GL.Clear(false, true, Color.clear);

        Graphics.Blit(src, blurred);

        for (int i = 0; i < 2; i++) {
            var temp = RenderTexture.GetTemporary(blurred.width, blurred.height);
            Graphics.Blit(blurred, temp, blurMaterial, 0);
            Graphics.Blit(temp, blurred, blurMaterial, 1);
            RenderTexture.ReleaseTemporary(temp);
        }
    }


    private RenderTexture CreateRenderTexture(int width, int height, int downscaleFactor, int depth, FilterMode filterMode) {
        RenderTexture temp = new RenderTexture(width >> downscaleFactor, height >> downscaleFactor, depth);
        temp.filterMode = filterMode;
        return temp;
    }
}
