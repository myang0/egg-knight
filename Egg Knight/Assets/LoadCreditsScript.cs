using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadCreditsScript : MonoBehaviour {
    public Image blackOut;

    public void StartLoadCredits() {
        StartCoroutine(LoadCredits());
    }
    
    private IEnumerator LoadCredits() {
        blackOut.gameObject.SetActive(true);
        bool isFading = true;
        float timeCurr = 0f;
        float timeDest = 2.5f;
        
        while (isFading) {
            timeCurr += Time.deltaTime / timeDest;
        
            Color transparent = new Color(0, 0, 0, 0);
            Color opaque = new Color(0, 0, 0, 1);
                
            blackOut.color = Color.Lerp(transparent, opaque, timeCurr);
            if (Math.Abs(blackOut.color.a - 1) < 0.01f) isFading = false;
            yield return null;
        }
        SceneManager.LoadScene("Scenes/EndScene");
    }
}
