using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleMenu : MonoBehaviour {
    public AsyncOperation loadingOperation;
    public GameObject loadingScreen;
    public CanvasGroup loadingCanvasGroup;
    public CanvasGroup titleCanvasGroup;
    public Slider loadingSlider;
    public bool isGameLoading;

    public void LoadIntro() {
        SceneManager.LoadScene("Scenes/IntroScene");
    }

    public void StartGame() {
        Debug.Log("STARTING GAME");
        StartCoroutine(FadeOutTitleScreen());
        isGameLoading = true;
        loadingOperation = SceneManager.LoadSceneAsync("Scenes/LevelsScene");
    }

    private IEnumerator FadeOutTitleScreen() {
        float loadingStartVal = loadingCanvasGroup.alpha;
        float titleStartVal = titleCanvasGroup.alpha;
        float time = 0;
        float duration = 0.33f;

        while (time < duration)
        {
            loadingCanvasGroup.alpha = Mathf.Lerp(loadingStartVal, 1, time / duration);
            titleCanvasGroup.alpha = Mathf.Lerp(titleStartVal, 0, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        loadingCanvasGroup.alpha = 1;
        titleCanvasGroup.alpha = 0;
        titleCanvasGroup.gameObject.SetActive(false);
    }

    public void Update() {
        if (!isGameLoading) return;
        if (loadingOperation != null) loadingSlider.value = Mathf.Clamp01(loadingOperation.progress / 0.9f);
    }

    public void QuitGame() {
        Debug.Log("QUITTING GAME");
        Application.Quit();
    }
}
