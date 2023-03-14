using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour {
    private Slider slider;
    [SerializeField] bool isBGM;
    
    void Awake() {
        slider = GetComponent<Slider>();
        if (isBGM) {
            slider.value = PlayerPrefs.GetFloat(PlayerPrefsKeys.BGM_SLIDER);
        } else {
            slider.value = PlayerPrefs.GetFloat(PlayerPrefsKeys.SFX_SLIDER);
        }
    }

    void Update() {
        SoundManager.Instance.SetVolume(slider.value, isBGM);
        if (isBGM) {
            PlayerPrefs.SetFloat(PlayerPrefsKeys.BGM_SLIDER, slider.value);
        } else {
            PlayerPrefs.SetFloat(PlayerPrefsKeys.SFX_SLIDER, slider.value);
        }
    }
}
