using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixelPerfectScript : MonoBehaviour {

    [System.Serializable]
    public struct PixelPerfectProfile {
        public int vertResolution;
        public int ppu;
    }

    public List<Camera> cameras;
    public float ppu;

    public List<PixelPerfectProfile> profiles;

    private float currentOrthographicSize;
    private Resolution currentResolution;

    private void Awake() {
        InitScreen();
    }

    public void InitScreen() {
        int screenHeight = Screen.height;
        float orthographicSize = screenHeight / ppu;
        while (orthographicSize >= 8) {
            orthographicSize /= 2.0f;
        }
        foreach (Camera camera in cameras) {
            camera.orthographicSize = orthographicSize;
        }
        //Debug.Log("Screen: (" + Screen.width + ", " + Screen.height + ")");
        //Debug.Log("Orthographic size: " + orthographicSize);
    }
}
