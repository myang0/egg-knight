using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DyingGuard : MonoBehaviour
{
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
            if (Input.GetKey(KeyCode.F)) StartEvent();
        }
    }

    private void StartEvent() {
        _hasEventExecuted = true;
        _waveCounterText.SetText("", 0);
        Fungus.Flowchart.BroadcastFungusMessage ("DyingGuard");
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player") && !_hasEventExecuted) {
            _isPlayerInRange = true;
            _waveCounterText.SetText("Press F to speak to Egg Guard Shelldon.", 0);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            _isPlayerInRange = false;
            _waveCounterText.SetText("", 0);
        }
    }

    public void DramaticDieMove() {
        StartCoroutine(BeginDeath());
    }

    private IEnumerator BeginDeath() {
        GetComponent<Collider2D>().enabled = false;
    
        Quaternion newRotation = Quaternion.Euler(0, 0, 90);
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        var newPos = transform.position;
        transform.position = new Vector3(newPos.x, newPos.y, ZcoordinateConsts.Interactable);
        sr.sortingLayerName = "Object";

        while (sr.color.a > 0) {
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * 7.5f);
            var color = sr.color;
            float newAlpha = color.a -= 0.001f;
            sr.color = new Color(color.r, color.g, color.b, newAlpha);
            yield return null;
        }
    }
}
