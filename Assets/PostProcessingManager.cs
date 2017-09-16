using System.Collections;
using System.Collections.Generic;
using UnityEngine.PostProcessing;
using UnityEngine;

public class PostProcessingManager : MonoBehaviour {

    public enum EffectModel : int {
        GRAIN, COLOR_GRADING, CHROMATIC_ABERRATION
    }

    public PostProcessingProfile defaultProfile;
    public Transform cameraTransform;

    private PostProcessingProfile currentProfile;

    private GrainModel.Settings[] grainSettings;
    private ColorGradingModel.Settings[] colorGradingSettings;
    private ChromaticAberrationModel.Settings[] chromaticAberrationSettings;

    private static int DEFAULT = 0;
    private static int CURRENT = 1;

    private void Awake() {
        grainSettings = new GrainModel.Settings[2];
        colorGradingSettings = new ColorGradingModel.Settings[2];
        chromaticAberrationSettings = new ChromaticAberrationModel.Settings[2];

        grainSettings[DEFAULT] = defaultProfile.grain.settings;
        colorGradingSettings[DEFAULT] = defaultProfile.colorGrading.settings;
        chromaticAberrationSettings[DEFAULT] = defaultProfile.chromaticAberration.settings;

        currentProfile = Instantiate(defaultProfile);
        cameraTransform.GetComponent<PostProcessingBehaviour>().profile = currentProfile;

        grainSettings[CURRENT] = currentProfile.grain.settings;
        colorGradingSettings[CURRENT] = currentProfile.colorGrading.settings;
        chromaticAberrationSettings[CURRENT] = currentProfile.chromaticAberration.settings;
    }

    // Use this for initialization
    void Start() {

    }

    private float elapsed = 1.0f;

    // Update is called once per frame
    void Update() {
        elapsed += Time.deltaTime / 3;
        UpdateSaturation(elapsed);
    }

    public void UpdateSaturation(float value) {
        value = Mathf.Clamp(value, 0, 2);
        colorGradingSettings[CURRENT].basic.saturation = value;
        currentProfile.colorGrading.settings = colorGradingSettings[CURRENT];
    }


}
