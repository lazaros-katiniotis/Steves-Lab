using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PlayerTexturePass : MonoBehaviour {

    private RenderTexture playerTexturePass;

    private string _playerTexture = "_PlayerTex";

    private void OnEnable() {
        Camera camera = GetComponent<Camera>();
        Shader playerShader = Shader.Find("Custom/PlayerTexture");
        if (camera.targetTexture != null) {
            RenderTexture temp = camera.targetTexture;
            camera.targetTexture = null;
            DestroyImmediate(temp);
        }

        playerTexturePass = CreateRenderTexture(camera.pixelWidth, camera.pixelHeight, 0, 24, FilterMode.Point);
        playerTexturePass.antiAliasing = 1;

        camera.targetTexture = playerTexturePass;
        camera.SetReplacementShader(playerShader, "");
        Shader.SetGlobalTexture(_playerTexture, playerTexturePass);
    }

    private RenderTexture CreateRenderTexture(int width, int height, int downscaleFactor, int depth, FilterMode filterMode) {
        RenderTexture temp = new RenderTexture(width >> downscaleFactor, height >> downscaleFactor, depth);
        temp.filterMode = filterMode;
        return temp;
    }
}
