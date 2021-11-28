using System;
using Stage;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour {
  private Animator _anim;
  private SpriteRenderer _sr;

  private bool _iFramesActive = false;

  private bool _isRolling = false;
  private float _anglesPerFrame = 0.0f;

  private void Awake() {
    PlayerMovement.OnRollBegin += HandleRollBegin;
    PlayerMovement.OnRollEnd += HandleRollEnd;

    PlayerMovement.OnMovementBegin += HandleMovement;
    PlayerMovement.OnMovementEnd += HandleStop;

    LevelManager.OnDialogueStart += HandleStop;

    PlayerHealth.OnIFramesEnabled += HandleOnIFramesEnabled;
    PlayerHealth.OnIFramesDisabled += HandleOnIFramesDisabled;

    PlayerHealth.OnNinjaIFramesEnabled += HandleNinjaIFramesEnabled;
    PlayerHealth.OnNinjaIFramesDisabled += HandleNinjaIFramesDisabled;

    _sr = gameObject.GetComponent<SpriteRenderer>();
    _anim = gameObject.GetComponent<Animator>();
  }

  private void Update() {
    if (GetComponent<PlayerInventory>().HasItem(Item.QuailEgg))
      transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
  }

  private void HandleRollBegin(object sender, RollEventArgs e) {
    _isRolling = true;

    float framesPerSecond = 50;
    _anglesPerFrame = (360.0f / (framesPerSecond * e.duration)) * ((e.direction == Direction.Right) ? -1 : 1);
  }

  private void HandleRollEnd(object sender, EventArgs e) {
    transform.eulerAngles = Vector3.zero;
    _isRolling = false;
  }

  private void HandleMovement(object sender, EventArgs e) {
    StartMovement();
  }

  private void HandleStop(object sender, EventArgs e) {
    StopMovement();
  }

  private void HandleOnIFramesEnabled(object sender, EventArgs e) {
    _iFramesActive = true;
  }

  private void HandleOnIFramesDisabled(object sender, EventArgs e) {
    _iFramesActive = false;
    _sr.color = new Color(1, 1, 1, 1);
  }

  private void HandleNinjaIFramesEnabled(object sender, EventArgs e) {
    _sr.color = new Color(1, 1, 1, 0.25f);
  }

  private void HandleNinjaIFramesDisabled(object sender, EventArgs e) {
    _sr.color = new Color(1, 1, 1, 1);
  }

  private void StartMovement() {
    if (_anim == null) {
      return;
    }

    if (_anim.GetBool("Moving") == false) {
      _anim.SetBool("Moving", true);
    }
  }

  private void StopMovement() {
    if (_anim == null) {
      return;
    }

    if (_anim.GetBool("Moving") == true) {
      _anim.SetBool("Moving", false);
    }
  }

  private void FixedUpdate() {
    Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

    _sr.flipX = (mousePos.x < transform.position.x);

    if (_iFramesActive) {
      float alpha = _sr.color.a;

      _sr.color = new Color(1, 1, 1, (alpha == 1) ? 0.25f : 1);
    }

    if (_isRolling) {
      transform.Rotate(0, 0, _anglesPerFrame);
    }
  }

  private void OnDestroy() {
    PlayerMovement.OnRollBegin -= HandleRollBegin;
    PlayerMovement.OnRollEnd -= HandleRollEnd;
    PlayerMovement.OnMovementBegin -= HandleMovement;
    PlayerMovement.OnMovementEnd -= HandleStop;
    LevelManager.OnDialogueStart -= HandleStop;
    PlayerHealth.OnIFramesEnabled -= HandleOnIFramesEnabled;
    PlayerHealth.OnIFramesDisabled -= HandleOnIFramesDisabled;
  }
}
