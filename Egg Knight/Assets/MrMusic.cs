using System;
using System.Collections;
using System.Collections.Generic;
using Fungus;
using Stage;
using UnityEngine;
using EventHandler = System.EventHandler;

public class MrMusic : MonoBehaviour {
    public AudioClip level1BGM;
    public AudioClip level2BGM;
    public AudioClip level3BGM;
    public AudioClip level1Boss;
    public AudioClip level2Boss;
    public AudioClip level3Boss;
    public AudioClip gameOver;

    public AudioSource source;
    public WriterAudio wAudio;

    public float musicVolume;
    public float fxVolume;

    [SerializeField] private float _volumeChange = 0.025f;
    [SerializeField] private float _musicFadeSpeed = 0.05f;
    [SerializeField] private float _timeBetweenTracks = 0.5f;

    private LevelManager _levelManager;

    public EventHandler OnSFXVolumeChange;
    
    void Start() {
        source.clip = level1BGM;
        source.loop = true;
        source.Play();
        _levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
        _levelManager.OnStageStart += StartLevelMusicSubscriber;
        OnSFXVolumeChange += (sender, args) => {
            Debug.Log("SFX Volume set to: " + fxVolume);
        };
        source.volume = PlayerPrefs.GetFloat("BGMVolume", 0.2f);
        musicVolume = source.volume;
        fxVolume = PlayerPrefs.GetFloat("SFXVolume", 0.2f);
        wAudio.volume = fxVolume * 2f;
        PlayerHealth.OnGameOver += StartGameOverMusic;
    }
    
    public void SetBackgroundMusicVolume(System.Single vol) {
        source.volume = vol;
        musicVolume = (float)vol;

        PlayerPrefs.SetFloat("BGMVolume", source.volume);
        PlayerPrefs.Save();
    }
    
    public void SetSFXVolume(System.Single vol) {
        OnSFXVolumeChange?.Invoke(this, EventArgs.Empty);
        fxVolume = vol;
        wAudio.volume = fxVolume * 2f;
        PlayerPrefs.SetFloat("SFXVolume", fxVolume);
        PlayerPrefs.Save();
    }

    void StartGameOverMusic(object sender, EventArgs e) {
        // StartCoroutine(MusicFadeOut(gameOver));
        source.clip = gameOver;
        source.loop = true;
        source.Play();
    }

    void StartBossMusic() {
        AudioClip clip = level1Boss;

        switch (_levelManager.level) {
            case 1:
                clip = level1Boss;
                break;
            case 2:
                clip = level2Boss;
                break;
            case 3:
                clip = level3Boss;
                break;
        }

        StartCoroutine(MusicFadeOut(clip));
    }

    void StartLevelMusicSubscriber(object sender, EventArgs e) {
        StartLevelMusic();
    }

    void StartLevelMusic() {
        var curLevel = _levelManager.level;
        if (curLevel == 1 && source.clip == level1BGM ||
            curLevel == 2 && source.clip == level2BGM ||
            curLevel == 3 && source.clip == level3BGM) return;

        AudioClip clip = level1BGM;
        
        if (curLevel == 1) clip = level1BGM;
        else if (curLevel == 2) clip = level2BGM;
        else if (curLevel == 3) clip = level3BGM;

        StartCoroutine(MusicFadeOut(clip));
    }

    public void StartMusicFadeOutNoClip() {
        StartCoroutine(MusicFadeOutNoClip());
    }
    
    private IEnumerator MusicFadeOutNoClip() {
        bool isFading = true;
        float timeCurr = 0f;
        float timeDest = 2.5f;

        float initialVolume = source.volume;
        while (isFading) {
            timeCurr += Time.deltaTime / timeDest;

            source.volume = Mathf.Lerp(initialVolume, 0f, timeCurr);
            if (Math.Abs(source.volume - 0) < 0.01f) isFading = false;
            yield return null;
        }
    }

    private IEnumerator MusicFadeOut(AudioClip clip) {
        float currentVol = source.volume;

        while (currentVol > 0) {
            currentVol = (currentVol - _volumeChange < 0) ? 0 : currentVol - _volumeChange;
            source.volume = currentVol;

            yield return new WaitForSeconds(_musicFadeSpeed);
        }

        yield return new WaitForSeconds(_timeBetweenTracks);

        StartCoroutine(MusicFadeIn(clip));
    }

    private IEnumerator MusicFadeIn(AudioClip clip) {
        source.clip = clip;
        source.loop = true;
        source.Play();

        float currentVol = source.volume;
        
        while (currentVol < musicVolume) {
            currentVol = (currentVol + _volumeChange > musicVolume) ? musicVolume : currentVol + _volumeChange;
            source.volume = currentVol;

            yield return new WaitForSeconds(_musicFadeSpeed);
        } 
    }

    private void OnDestroy() {
        PlayerHealth.OnGameOver -= StartGameOverMusic;
    }
}
