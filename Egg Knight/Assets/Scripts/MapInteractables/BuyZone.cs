using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyZone : MonoBehaviour {
    public bool isPlayerInBuyZone;
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) isPlayerInBuyZone = true;
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            isPlayerInBuyZone = false;
            WaveCounterText _waveCounterText = FindObjectOfType<WaveCounterText>();
            _waveCounterText.SetText("", 0);
        }
    }
}
