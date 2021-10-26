using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Campfire : MonoBehaviour {
    private bool _hasEventExecuted;
    private bool _isPlayerInRange;
    private WaveCounterText _waveCounterText;
    void Start()
    {
        _waveCounterText = FindObjectOfType<WaveCounterText>();
        Vector3 currPos = transform.position;
        transform.position = new Vector3(currPos.x, currPos.y, ZcoordinateConsts.Interactable);
    }

    // Update is called once per frame
    void Update()
    {
        if (_isPlayerInRange && !_hasEventExecuted) {
            if (Input.GetKey(KeyCode.F)) StartRestEvent();
        }
    }

    private void StartRestEvent() {
        _hasEventExecuted = true;
        _waveCounterText.SetText("", 0);
        Fungus.Flowchart.BroadcastFungusMessage ("Rest");
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>().Heal(100f);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            _isPlayerInRange = true;
            _waveCounterText.SetText("Press F to rest by the campfire.", 0);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            _isPlayerInRange = false;
            _waveCounterText.SetText("", 0);
        }
    }
}
