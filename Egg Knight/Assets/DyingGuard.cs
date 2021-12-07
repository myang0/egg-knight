using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DyingGuard : MonoBehaviour
{
    private Animator _anim;

    private bool _hasEventExecuted;
    private bool _isPlayerInRange;
    private bool dying;
    private WaveCounterText _waveCounterText;
    void Start()
    {
        _anim = GetComponent<Animator>();

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

    private void FixedUpdate() {
        if (dying) {
            if (_anim.GetBool("IsDead") == false) {
                _anim.SetBool("IsDead", true);
            }

            GetComponent<Collider2D>().enabled = false;
    
            Quaternion newRotation = Quaternion.Euler(0, 0, 90);
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            var newPos = transform.position;
            transform.position = new Vector3(newPos.x, newPos.y, ZcoordinateConsts.Interactable);
            sr.sortingLayerName = "Object";

            if (sr.color.a > 0) {
                transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * 7.5f);
                var color = sr.color;
                float newAlpha = color.a -= Time.deltaTime*0.5f;
                sr.color = new Color(color.r, color.g, color.b, newAlpha);
            }
            else {
                Destroy(gameObject);
            }
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
        dying = true;
    }
}
