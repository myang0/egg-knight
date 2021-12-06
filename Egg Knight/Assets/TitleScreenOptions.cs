using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreenOptions : MonoBehaviour
{
    public void SetBackgroundMusicVolume(System.Single vol) {
        PlayerPrefs.SetFloat("BGMVolume", vol);
        PlayerPrefs.Save();
    }
    
    public void SetSFXVolume(System.Single vol) {
        PlayerPrefs.SetFloat("SFXVolume", vol);
        PlayerPrefs.Save();
    }
}
