using System;
using System.Collections;
using System.Collections.Generic;
using Stage;
using UnityEngine;

public class MrMusic : MonoBehaviour {
    public AudioClip level1BGM;
    public AudioClip level2BGM;
    public AudioClip level3BGM;
    public AudioClip level1Boss;
    public AudioClip level2Boss;
    public AudioClip level3Boss;

    public AudioSource source;

    private LevelManager _levelManager;
    // Start is called before the first frame update
    void Start() {
        source.clip = level1BGM;
        source.loop = true;
        source.Play();
        _levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
        _levelManager.OnStageStart += StartLevelMusicSubscriber;
    }

    void StartBossMusic() {
        switch (_levelManager.level) {
            case 1:
                source.clip = level1Boss;
                break;
            case 2:
                source.clip = level2Boss;
                break;
            case 3:
                source.clip = level3Boss;
                break;
        }
        source.loop = true;
        source.Play();
    }

    void StartLevelMusicSubscriber(object sender, EventArgs e) {
        StartLevelMusic();
    }

    void StartLevelMusic() {
        var curLevel = _levelManager.level;
        if (curLevel == 1 && source.clip == level1BGM ||
            curLevel == 2 && source.clip == level2BGM ||
            curLevel == 3 && source.clip == level3BGM) return;
        
        if (curLevel == 1) source.clip = level1BGM;
        else if (curLevel == 2) source.clip = level2BGM;
        else if (curLevel == 3) source.clip = level3BGM;

        source.loop = true;
        source.Play();
    }
}
