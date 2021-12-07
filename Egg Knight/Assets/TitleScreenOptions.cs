using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleScreenOptions : MonoBehaviour {
    public Toggle toggle;

    public void Awake() {
        bool on = PlayerPrefs.GetInt(StatDisplayManager.StatDisplayKey, 1) == 1;
        toggle.isOn = on;
    }

    public void SetBackgroundMusicVolume(System.Single vol) {
        PlayerPrefs.SetFloat("BGMVolume", vol);
        PlayerPrefs.Save();
    }
    
    public void SetSFXVolume(System.Single vol) {
        PlayerPrefs.SetFloat("SFXVolume", vol);
        PlayerPrefs.Save();
    }

    public void SetEnableStatDisplay(bool on) {
        if (on) PlayerPrefs.SetInt(StatDisplayManager.StatDisplayKey, 1);
        else PlayerPrefs.SetInt(StatDisplayManager.StatDisplayKey, 0);
        PlayerPrefs.Save();
    }
}
