using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class DeathScreen : MonoBehaviour {
    public GameObject toBeContinued;
    public List<GameObject> uiToHide = new List<GameObject>();
    public GameObject deathOverlay;
    
    private void Awake() {
        PlayerDeath.OnDeathScreenStart += AnimateToBeContinuedSubscriber;
    }

    private void AnimateToBeContinuedSubscriber(object sender, EventArgs e) {
        StartCoroutine(AnimateToBeContinued());
        foreach (var v in uiToHide) {
            v.SetActive(false);
        }
    }

    private IEnumerator AnimateToBeContinued() {
        RectTransform tbcRectTransform = toBeContinued.GetComponent<RectTransform>();
        var initialPos = tbcRectTransform.anchoredPosition;
        bool isAnimating = true;
        float timeDest = 0.15f;
        float timeCurr = 0;
        while (isAnimating) {
            timeCurr += Time.deltaTime / timeDest;
            tbcRectTransform.anchoredPosition =
                Vector2.Lerp(initialPos, Vector2.zero, timeCurr);
            if (tbcRectTransform.anchoredPosition.x <= Vector2.zero.x) isAnimating = false;
            yield return new WaitForFixedUpdate();
        }
        deathOverlay.SetActive(true);
        Time.timeScale = 0;
        yield return null;
    }

    private void OnDestroy() {
        PlayerDeath.OnDeathScreenStart -= AnimateToBeContinuedSubscriber;
    }
}
