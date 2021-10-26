using System.Collections;
using System.Collections.Generic;
using Stage;
using UnityEngine;

public class Grave : MonoBehaviour
{
    private bool _hasEventExecuted;
    private bool _isPlayerInRange;
    private WaveCounterText _waveCounterText;
    public SpriteRenderer _rachaSR;
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
        LevelManager levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
        if (levelManager.numSirRachaVisits < 3) {
            Fungus.Flowchart.BroadcastFungusMessage ("SirRacha" + levelManager.numSirRachaVisits);
        }
        else {
            Fungus.Flowchart.BroadcastFungusMessage ("SirRachaRepeat");
        }

        var color = _rachaSR.color;
        _rachaSR.color = new Color(color.r, color.b, color.g, 1);
    }

    public void HideRacha() {
        var color = _rachaSR.color;
        _rachaSR.color = new Color(color.r, color.b, color.g, 0);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player") && !_hasEventExecuted) {
            _isPlayerInRange = true;
            _waveCounterText.SetText("Press F to pay respects to Sir Racha's grave", 0);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            _isPlayerInRange = false;
            _waveCounterText.SetText("", 0);
        }
    }
}
