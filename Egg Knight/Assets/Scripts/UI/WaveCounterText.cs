using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaveCounterText : MonoBehaviour
{
    private TextMeshProUGUI _display;

    private void Awake() {
        _display = gameObject.GetComponent<TextMeshProUGUI>();
        

        _display.text = "";
    }

    public void ChangeText(String newText) {
        _display.text = newText;
    }
}
