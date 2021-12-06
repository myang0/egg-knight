using System;
using System.Collections;
using System.Collections.Generic;
using Stage;
using UnityEngine;

public class TitleMenuMusic : MonoBehaviour {
    public AudioClip menuMusic;
    public AudioSource source;

    void Start() {
        source.clip = menuMusic;
        source.loop = true;
        source.Play();
        source.volume = PlayerPrefs.GetFloat("BGMVolume", 0.2f);
    }
    
    public void SetBackgroundMusicVolume(System.Single vol) {
        source.volume = vol;

        PlayerPrefs.SetFloat("BGMVolume", source.volume);
        PlayerPrefs.Save();
    }
}
