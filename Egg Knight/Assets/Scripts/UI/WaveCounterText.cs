using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaveCounterText : MonoBehaviour
{
    private static TextMeshProUGUI _display;
    private float _messageDuration;

    private void Awake() {
        _display = gameObject.GetComponent<TextMeshProUGUI>();
        _display.text = "";
    }

    public void SetText(String newText, float duration) {
        _display.text = newText;
        if (duration > 0) {
            _messageDuration = duration;
            StartCoroutine(RemoveTextDelay());
        }
        else {
            _messageDuration = 0;
        }
    }

    private IEnumerator RemoveTextDelay() {
        yield return new WaitForSeconds(_messageDuration);
        _display.text = "";
    }
}
