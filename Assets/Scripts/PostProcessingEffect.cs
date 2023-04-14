using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessingEffect : MonoBehaviour {
    public static PostProcessingEffect Instance { get; private set; }

    [SerializeField] private Volume globalVolume;
    private float initialVignetteIntensity;
    private float initialChromaticAbberaionIntensity;
    private ClampedFloatParameter initialColorFilterGB;
    private ClampedFloatParameter initialMotionBlurClamp;

    void Awake () {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    void Start() {
        Vignette vignette;
        globalVolume.profile.TryGet<Vignette>(out vignette);
        initialVignetteIntensity = vignette.intensity.value;

        ChromaticAberration chromaticAberration;
        globalVolume.profile.TryGet<ChromaticAberration>(out chromaticAberration);
        initialChromaticAbberaionIntensity = chromaticAberration.intensity.value;

        initialColorFilterGB = new ClampedFloatParameter(1, 0, 1, true);
        initialMotionBlurClamp = new ClampedFloatParameter(0, 0, 0.2f, true);
    }

    public void DyingEffect(float amount, bool decrease = false) {
        Vignette vignette;
        globalVolume.profile.TryGet<Vignette>(out vignette);
        ClampedFloatParameter tempVignette = vignette.intensity;
        if (decrease) {
            tempVignette.value -= amount;
        } else {
            tempVignette.value += amount;
        }
        vignette.intensity = tempVignette;

        ChromaticAberration chromaticAberration;
        globalVolume.profile.TryGet<ChromaticAberration>(out chromaticAberration);
        ClampedFloatParameter tempChromaticAberration = chromaticAberration.intensity;
        if (decrease) {
            tempChromaticAberration.value -= (amount * 2);
        } else {
            tempChromaticAberration.value += (amount * 2);
        }
        chromaticAberration.intensity = tempChromaticAberration;

        ColorAdjustments colorAdjustments;
        globalVolume.profile.TryGet<ColorAdjustments>(out colorAdjustments);
        ColorParameter tempColorFilter = colorAdjustments.colorFilter;
        if (decrease) {
            tempColorFilter.Interp(
                Color.white, 
                new Color(1, initialColorFilterGB.value += (amount * 2), initialColorFilterGB.value += (amount * 2), 1), 
                0.5f);
        } else {
            tempColorFilter.Interp(
                Color.white, 
                new Color(1, initialColorFilterGB.value -= (amount * 2), initialColorFilterGB.value -= (amount * 2), 1), 
                0.5f);
        }
        colorAdjustments.colorFilter = tempColorFilter;

        MotionBlur motionBlur;
        globalVolume.profile.TryGet<MotionBlur>(out motionBlur);
        ClampedFloatParameter tempMotionBlur = motionBlur.clamp;
        if (decrease) {
            tempMotionBlur.value -= (amount / 2);
        } else {
            tempMotionBlur.value += (amount / 2);
        }
        motionBlur.clamp = tempMotionBlur;
    }

    public void ResetDyingEffect() {
        Vignette vignette;
        globalVolume.profile.TryGet<Vignette>(out vignette);
        ClampedFloatParameter tempVignette = vignette.intensity;
        tempVignette.value = initialVignetteIntensity;
        vignette.intensity = tempVignette;

        ChromaticAberration chromaticAberration;
        globalVolume.profile.TryGet<ChromaticAberration>(out chromaticAberration);
        ClampedFloatParameter tempChromaticAberration = chromaticAberration.intensity;
        tempChromaticAberration.value = initialChromaticAbberaionIntensity;
        chromaticAberration.intensity = tempChromaticAberration;

        ColorAdjustments colorAdjustments;
        globalVolume.profile.TryGet<ColorAdjustments>(out colorAdjustments);
        ColorParameter tempColorFilter = colorAdjustments.colorFilter;
        tempColorFilter.Interp(Color.white, Color.white, 0);
        colorAdjustments.colorFilter = tempColorFilter;

        MotionBlur motionBlur;
        globalVolume.profile.TryGet<MotionBlur>(out motionBlur);
        ClampedFloatParameter tempMotionBlur = motionBlur.intensity;
        tempMotionBlur.value = initialChromaticAbberaionIntensity;
        motionBlur.intensity = tempMotionBlur;
    }

}
