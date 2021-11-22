using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class IntroControls : MonoBehaviour
{
    public enum IntroState {
        splash, video
    }

    public VideoPlayer vPlayer;
    public SpriteRenderer wiener;
    public IntroState state;

    void Start() {
        StartCoroutine(StartSplashTimer());
        vPlayer.loopPointReached += EndReached;
    }

    private IEnumerator StartSplashTimer() {
        var t = 0f;
        var timeToMove = 1f;
        Color transparent = new Color(255, 255, 255, 0);
        Color opaque = new Color(255, 255, 255, 1);
        
        while(t < 1)
        {
            t += Time.deltaTime / timeToMove;
            wiener.color = Color.Lerp(transparent, opaque, t);
            yield return null;
        }
        
        yield return new WaitForSeconds(2f);

        t = 0f;
        while(t < 1)
        {
            t += Time.deltaTime / timeToMove;
            wiener.color = Color.Lerp(opaque, transparent, t);
            yield return null;
        }
        EndSplashScreen();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return)) {
            if (state == IntroState.splash) {
                EndSplashScreen();
            } else if (state == IntroState.video) {
                vPlayer.loopPointReached -= EndReached;
                SceneManager.LoadScene("Scenes/TitleScene");
            }
        }
    }

    private void EndSplashScreen() {
        StopCoroutine(StartSplashTimer());
        wiener.gameObject.SetActive(false);
        vPlayer.Play();   
        state = IntroState.video;

    }

    private void EndReached(VideoPlayer vp) {
        SceneManager.LoadScene("Scenes/TitleScene");
    }
}
