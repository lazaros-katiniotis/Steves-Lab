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
    private float oxygenatedElapsed = 1.0f;
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
        if (player.IsDead()) {
            ResetPostProcessingEffects();
        } else {
            OxygenationEffect();
        }
    }

    public void OxygenationEffect() {
        float currentSaturationValue = GetSaturationValue(CURRENT);
        if (player.IsOxygenDepleting()) {
            oxygenatedElapsed = 0.0f;
            deoxygenatedElapsed += Time.deltaTime / 4f;
            nextSaturationValue = GetScaledValue(player.playerOxygenLevel, 0.50f, 0.75f);
            currentSaturationValue = Mathf.Lerp(prevSaturationValue, nextSaturationValue, deoxygenatedElapsed);

            SetContrastValue(Mathf.Lerp(GetContrastValue(CURRENT), 1.0f, deoxygenatedElapsed * 4));
            SetGrainIntensityValue(Mathf.Lerp(GetGrainIntensityValue(CURRENT), 0.6f - GetScaledValue(player.playerOxygenLevel, 0, 0.6f), deoxygenatedElapsed));
            SetChromaticAbberationIntensityValue(Mathf.Lerp(GetChromaticAbberationIntensityValue(CURRENT), 0.75f - GetScaledValue(player.playerOxygenLevel, 0, 0.75f), deoxygenatedElapsed));
        } else {
            deoxygenatedElapsed = 0.0f;
            oxygenatedElapsed += Time.deltaTime / 6f;
            float curveValue = Mathf.Clamp(oxygenatedCurve.Evaluate(oxygenatedElapsed), 0, 1);
            float oxygenValue = Mathf.Clamp(player.playerOxygenLevel + 2.5f * curveValue, 0, 2.5f);
            nextSaturationValue = GetScaledValue(oxygenValue, 1.05f, 1.05f);
            currentSaturationValue = Mathf.Lerp(prevSaturationValue, nextSaturationValue, oxygenatedElapsed);
            SetContrastValue(GetScaledValue(curveValue, 1, 1.4f));
            SetGrainIntensityValue(Mathf.Lerp(GetGrainIntensityValue(CURRENT), 0.0f, oxygenatedElapsed * 3f));
            SetChromaticAbberationIntensityValue(Mathf.Lerp(GetChromaticAbberationIntensityValue(CURRENT), 0, oxygenatedElapsed * 3f));
        }
        SetSaturationValue(currentSaturationValue);
        prevSaturationValue = currentSaturationValue;
    }

    private void ResetPostProcessingEffects() {
        SetSaturationValue(GetSaturationValue(DEFAULT));
        SetContrastValue(GetContrastValue(DEFAULT));
        SetGrainIntensityValue(GetGrainIntensityValue(DEFAULT));
        SetChromaticAbberationIntensityValue(GetChromaticAbberationIntensityValue(DEFAULT));
    }

    public float GetScaledValue(float value, float min, float max) {
        float range = max - min;
        float newValue = min + value * range;
        return newValue;
    }

    public void SetSaturationValue(float value) {
        colorGradingSettings[CURRENT].basic.saturation = value;
        currentProfile.colorGrading.settings = colorGradingSettings[CURRENT];
    }

    public float GetSaturationValue(int id) {
        return colorGradingSettings[id].basic.saturation;
    }

    public void SetContrastValue(float value) {
        colorGradingSettings[CURRENT].basic.contrast = value;
        currentProfile.colorGrading.settings = colorGradingSettings[CURRENT];
    }

    public float GetContrastValue(int id) {
        return colorGradingSettings[id].basic.contrast;
    }

    public void SetGrainIntensityValue(float value) {
        grainSettings[CURRENT].intensity = value;
        currentProfile.grain.settings = grainSettings[CURRENT];
    }

    public float GetGrainIntensityValue(int id) {
        return grainSettings[id].intensity;
    }


    public void SetChromaticAbberationIntensityValue(float value) {
        chromaticAberrationSettings[CURRENT].intensity = value;
        currentProfile.chromaticAberration.settings = chromaticAberrationSettings[CURRENT];
    }

    public float GetChromaticAbberationIntensityValue(int id) {
        return chromaticAberrationSettings[id].intensity;
    }

}
