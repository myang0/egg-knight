using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderValueDisplay : MonoBehaviour {
    public TextMeshProUGUI tmp;
    public Slider slider;
    
    public SliderType sliderType;
    public enum SliderType {
        BGM,
        SFX
    };

    private void Awake() {
        if (sliderType == SliderType.BGM) {
            slider.value = PlayerPrefs.GetFloat("BGMVolume", 0.2f);
            tmp.text = "" + RoundValue(PlayerPrefs.GetFloat("BGMVolume", 0.2f));
        }

        if (sliderType == SliderType.SFX) {
            slider.value = PlayerPrefs.GetFloat("SFXVolume", 0.2f);
            tmp.text = "" + RoundValue(PlayerPrefs.GetFloat("SFXVolume", 0.2f));
        }
    }

    public int RoundValue(float val) {
        if (val < 1) return Mathf.RoundToInt(val*100);
        return Mathf.RoundToInt(val);
    }
    
    public void SetText(System.Single val) {
        tmp.text = "" + Mathf.RoundToInt(val*100);
    }
}
