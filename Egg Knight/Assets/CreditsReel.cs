using System;
using System.Collections;
using System.Collections.Generic;
using Fungus;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class CreditsReel : MonoBehaviour {
    public GameObject credits;
    public GameObject endPoint;
    public Image blackOut;
    public AudioSource source;
    public AudioClip creditsBGM;
    public WriterAudio wAudio;
    public bool isSpedUp;
    bool isRolling;

    private readonly Color _transparent = new Color(0, 0, 0, 0);
    private readonly Color _opaque = new Color(0, 0, 0, 1);
    
    private void Awake() {
        EggtonioCredits.OnOutroStart += StartFadeIn;
        EggtonioCredits.OnOutroEnd += StartCredits;
        wAudio.volume = PlayerPrefs.GetFloat("SFXVolume", 0.2f)*2f;
        source.clip = creditsBGM;
        source.loop = true;
    }

    private void StartFadeIn(object sender, EventArgs e) {
        source.Play();
        StartCoroutine(FadeIn());
    }

    private void Update() {
        if (!isRolling) return;
        if (Input.GetKeyDown(KeyCode.Space) ||
            Input.GetKeyDown(KeyCode.Escape) ||
            Input.GetMouseButtonDown(0)) {
            isSpedUp = !isSpedUp;
        }
    }

    private void StartCredits(object sender, EventArgs e) {
        StartCoroutine(RollCredits());
    }

    private IEnumerator RollCredits() {
        RectTransform creditsRectTransform = credits.GetComponent<RectTransform>();
        var initialPos = creditsRectTransform.anchoredPosition;
        var endPointPos = endPoint.GetComponent<RectTransform>().anchoredPosition;
        isRolling = true;
        float timeDest;
        float timeCurr = 0f;

        while (isRolling) {
            timeDest = isSpedUp ? 5f : 15f;
            timeCurr += Time.deltaTime / timeDest;
            creditsRectTransform.anchoredPosition =
                Vector2.Lerp(initialPos, endPointPos, timeCurr);
            if (creditsRectTransform.anchoredPosition.y >= endPointPos.y) isRolling = false;
            yield return new WaitForFixedUpdate();
        }

        StartCoroutine(LoadMainMenu());
    }

    private IEnumerator FadeIn() {
        bool isFading = true;
        float timeCurr = 0f;
        float timeDest = 2f;
        float targetVolume = PlayerPrefs.GetFloat("BGMVolume", 0.2f);
        
        while (isFading) {
            timeCurr += Time.deltaTime / timeDest;
            blackOut.color = Color.Lerp(_opaque, _transparent, timeCurr);
            source.volume = Mathf.Lerp(0, targetVolume, timeCurr);
            if (Math.Abs(blackOut.color.a - 0) < 0.01f) isFading = false;
            yield return null;
        }
    }

    private IEnumerator LoadMainMenu() {
        bool isFading = true;
        float timeCurr = 0f;
        float timeDest = isSpedUp ? 0.5f : 1.5f;
        float delay = isSpedUp ? 1f : 3f;
        
        yield return new WaitForSeconds(delay);
        
        while (isFading) {
            timeCurr += Time.deltaTime / timeDest;
            blackOut.color = Color.Lerp(_transparent, _opaque, timeCurr);
            if (Math.Abs(blackOut.color.a - 1) < 0.01f) isFading = false;
            yield return null;
        }
        SceneManager.LoadScene("Scenes/TitleScene");
    }

    private void OnDestroy() {
        EggtonioCredits.OnOutroStart -= StartFadeIn;
        EggtonioCredits.OnOutroEnd -= StartCredits;
    }
}
