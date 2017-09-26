using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GlowCameraScript : MonoBehaviour {

    private Camera camera;
    private int downResFactor = 0;

    private string globalTextureName = "_GlobalTestTex";

    private RenderTexture prePass;

    void GenerateRT() {
        camera = GetComponent<Camera>();

        if (camera.targetTexture != null) {
            RenderTexture temp = camera.targetTexture;
            camera.targetTexture = null;
            DestroyImmediate(temp);
        }

        prePass = new RenderTexture(camera.pixelWidth >> downResFactor, camera.pixelHeight >> downResFactor, 24);
        Shader glowShader = Shader.Find("Hidden/GlowReplace");
        camera.targetTexture = prePass;
        camera.SetReplacementShader(glowShader, "Glowable");
        camera.targetTexture.filterMode = FilterMode.Bilinear;
        Shader.SetGlobalTexture("_GlowPrePassTex", prePass);

        //Shader.SetGlobalTexture(globalTextureName, camera.targetTexture);
    }

    private void OnEnable() {
        GenerateRT();
    }
}
