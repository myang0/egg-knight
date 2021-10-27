using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eggitha : MonoBehaviour {
    public enum TutorialState {
        NotStarted,  
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
    public List<EnemyBehaviour> enemies = new List<EnemyBehaviour>();

    // Start is called before the first frame update
    void Awake() {
        _sr = GetComponent<SpriteRenderer>();
        _waveCounterText = FindObjectOfType<WaveCounterText>();
        HideEggitha();
        foreach (var enemy in enemies) {
            enemy.gameObject.SetActive(false);
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

        if (tutorialState == TutorialState.Attack) {
            if (enemies.Count == 0) StartCoroutine(DelayBeginPreComplete());
            else if (enemies[0] == null) enemies.Remove(enemies[0]);
        }
    }

    private void BeginPreMovement() {
        tutorialState = TutorialState.PreMovement;
        ShowEggitha();
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
        foreach (var enemy in enemies) {
            enemy.gameObject.SetActive(true);
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
    }

    private void BeginRush() {
        Fungus.Flowchart.BroadcastFungusMessage ("RushedTutorial");
        _waveCounterText.SetText("", 0);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player") && tutorialState == TutorialState.NotStarted) {
            BeginPreMovement();
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.collider.CompareTag("Player") && tutorialState != TutorialState.Complete) {
            BeginRush();
        }
    }

    private void HideEggitha() {
        var color = _sr.color;
        _sr.color = new Color(color.r, color.b, color.g, 0);
    }

    private void ShowEggitha() {
        var color = _sr.color;
        _sr.color = new Color(color.r, color.b, color.g, 1);
    }
}
