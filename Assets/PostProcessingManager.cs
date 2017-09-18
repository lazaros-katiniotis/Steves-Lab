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

    private PlayerController player;
    private PostProcessingProfile currentProfile;

    private GrainModel.Settings[] grainSettings;
    private ColorGradingModel.Settings[] colorGradingSettings;
    private ChromaticAberrationModel.Settings[] chromaticAberrationSettings;

    private static int DEFAULT = 0;
    private static int CURRENT = 1;

    public AnimationCurve oxygenatedCurve;
    private float oxygenatedElapsed = 0.0f;
    private float deoxygenatedElapsed = 0.0f;
    private float prevSaturationValue = 0.0f;
    private float nextSaturationValue = 0.0f;

    private void Awake() {
        player = GetComponent<PlayerController>();
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

    // Update is called once per frame
    void Update() {
        UpdateSaturation();
    }

    public void UpdateSaturation() {
        float currentSaturationValue = GetSaturationValue();
        if (player.IsOxygenDepleting()) {
            deoxygenatedElapsed += Time.deltaTime / 8f;
            nextSaturationValue = GetScaledSaturationValue(player.playerOxygenLevel, 0.55f, 1.05f);
            currentSaturationValue = Mathf.Lerp(prevSaturationValue, nextSaturationValue, deoxygenatedElapsed);
            oxygenatedElapsed = 0.0f;
        } else {
            oxygenatedElapsed += Time.deltaTime / 3;
            float curveValue = Mathf.Clamp(oxygenatedCurve.Evaluate(oxygenatedElapsed), 0, 1);
            nextSaturationValue = GetScaledSaturationValue(player.playerOxygenLevel + 2.5f * curveValue, 0.55f, 1.05f);
            currentSaturationValue = Mathf.Lerp(prevSaturationValue, nextSaturationValue, oxygenatedElapsed);
            deoxygenatedElapsed = 0.0f;
        }
        SetSaturationValue(currentSaturationValue);
        prevSaturationValue = currentSaturationValue;
    }

    public float GetScaledSaturationValue(float value, float min, float max) {
        float range = max - min;
        float newValue = min + value * range;
        return newValue;
    }

    public void SetSaturationValue(float value) {
        colorGradingSettings[CURRENT].basic.saturation = value;
        currentProfile.colorGrading.settings = colorGradingSettings[CURRENT];
    }

    public float GetSaturationValue() {
        return colorGradingSettings[CURRENT].basic.saturation;
    }


}
