using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eggitha : MonoBehaviour {
    public enum TutorialState {
        NotStarted,
        PreLore,
        Lore,
        PreMovement,
        Movement,
        PreAttack,
        Attack,
        PreComplete,
        Complete
    }
    public TutorialState tutorialState = TutorialState.NotStarted;
    private SpriteRenderer _sr;
    private WaveCounterText _waveCounterText;
    public bool hasMovedUp;
    public bool hasMovedDown;
    public bool hasMovedLeft;
    public bool hasMovedRight;
    public bool hasRolled;
    public bool playerInRange;
    public List<TrainingDummyBehavior> dummies = new List<TrainingDummyBehavior>();

    // Start is called before the first frame update
    void Awake() {
        _sr = GetComponent<SpriteRenderer>();
        _waveCounterText = FindObjectOfType<WaveCounterText>();
        _sr.color = new Color(255, 255, 255, 0);
        foreach (var dummy in dummies) {
            dummy.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (tutorialState == TutorialState.Movement) {
            if (Input.GetKey(KeyCode.W) && !hasMovedUp) hasMovedUp = true;
            if (Input.GetKey(KeyCode.S) && !hasMovedDown) hasMovedDown = true;
            if (Input.GetKey(KeyCode.A) && !hasMovedLeft) hasMovedLeft = true;
            if (Input.GetKey(KeyCode.D) && !hasMovedRight) hasMovedRight = true;
            if (Input.GetKey(KeyCode.Space) && !hasRolled) hasRolled = true;
            if (hasMovedUp && hasMovedDown && hasMovedLeft && hasMovedRight && hasRolled)
                StartCoroutine(DelayBeginPreAttack());
        }
        
        else if (tutorialState == TutorialState.PreLore && Input.GetKey(KeyCode.F) && playerInRange) {
            BeginLore();
        }

        else if (tutorialState == TutorialState.Attack) {
            if (dummies.Count == 0) StartCoroutine(DelayBeginPreComplete());
            else if (dummies[0] == null) dummies.Remove(dummies[0]);
        }
    }

    private void BeginPreLore() {
        GetComponent<CircleCollider2D>().radius = 2.5f;
        ShowEggitha();
        tutorialState = TutorialState.PreLore;
        _waveCounterText.SetText("", 0);
        Fungus.Flowchart.BroadcastFungusMessage ("Intro");
    }

    private void BeginLore() {
        tutorialState = TutorialState.Lore;
        _waveCounterText.SetText("", 0);
        Fungus.Flowchart.BroadcastFungusMessage ("TalkToEggitha");
    }

    private void BeginPreMovement() {
        tutorialState = TutorialState.PreMovement;
        Fungus.Flowchart.BroadcastFungusMessage ("MovementTutorial");
        _waveCounterText.SetText("", 0);
    }

    private void BeginMovement() {
        tutorialState = TutorialState.Movement;
        _waveCounterText.SetText("Use WASD to move! Press SPACE while moving to roll!", 0);
    }
    
    IEnumerator DelayBeginPreAttack() {
        tutorialState = TutorialState.PreAttack;
        yield return new WaitForSeconds(1f);
        BeginPreAttack();
    }
    
    private void BeginPreAttack() {
        Fungus.Flowchart.BroadcastFungusMessage ("AttackTutorial");
        _waveCounterText.SetText("", 0);
    }

    private void BeginAttack() {
        tutorialState = TutorialState.Attack;
        foreach (var dummy in dummies) {
            dummy.gameObject.SetActive(true);
            dummy.RevealSelf();
        }
        _waveCounterText.SetText("Left click to attack! Press Q and E to swap weapons! Right click to fire yolk!", 0);
    }
    
    IEnumerator DelayBeginPreComplete() {
        tutorialState = TutorialState.PreComplete;
        yield return new WaitForSeconds(1f);
        BeginPreComplete();
    }

    private void BeginPreComplete() {
        Fungus.Flowchart.BroadcastFungusMessage ("CompleteTutorial");
        _waveCounterText.SetText("", 0);
    }

    private void BeginComplete() {
        tutorialState = TutorialState.Complete;
        GetComponent<BoxCollider2D>().isTrigger = true;
        HideEggitha();
        foreach (var dummy in dummies) {
            if (dummy) Destroy(dummy.gameObject);
        }
    }

    private void BeginRush() {
        Fungus.Flowchart.BroadcastFungusMessage ("RushedTutorial");
        _waveCounterText.SetText("", 0);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (!other.CompareTag("Player") || other.isTrigger) return;
        
        if (tutorialState == TutorialState.NotStarted) BeginPreLore();
        if (tutorialState == TutorialState.PreLore) {
            _waveCounterText.SetText("Press F to speak to Queen Eggitha", 0);
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (tutorialState == TutorialState.PreLore) {
            _waveCounterText.SetText("", 0);
            playerInRange = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.collider.CompareTag("Player") && tutorialState != TutorialState.Complete) {
            BeginRush();
        }
    }

    private void HideEggitha() {
        StartCoroutine(DecreaseEggithaAlpha());
    }

    private void ShowEggitha() {
        StartCoroutine(IncreaseEggithaAlpha());
    }

    private IEnumerator IncreaseEggithaAlpha() {
        var color = _sr.color;
        float newAlpha = color.a;
        while (newAlpha < 1) {
            newAlpha += 0.0015f;
            _sr.color = new Color(color.r, color.b, color.g, newAlpha);
            yield return null;
        }
    }
    
    private IEnumerator DecreaseEggithaAlpha() {
        var color = _sr.color;
        float newAlpha = color.a;
        while (newAlpha > 0) {
            newAlpha -= 0.002f;
            _sr.color = new Color(color.r, color.b, color.g, newAlpha);
            yield return null;
        }
    }
}
