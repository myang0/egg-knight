using System;
using Stage;
using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
  private Rigidbody2D _rb;

  [SerializeField] private float _movementSpeed = 5.0f;
  public float _currentMovementSpeed;

  [SerializeField] private float _rollSpeed = 15.0f;
  [SerializeField] private float _rollDrag = 3.0f;
  [SerializeField] private float _rollDuration = 0.5f;

  [SerializeField] private float _rollCooldown = 0.25f;
  private bool _canRoll = true;
  private bool _isVisible = true;

  private Vector2 _lastMovementVector;

  public static event EventHandler OnMovementBegin;
  public static event EventHandler OnMovementEnd;
  public static event EventHandler OnSpeedChange;

  public static event EventHandler<RollEventArgs> OnRollBegin;
  public static event EventHandler OnRollEnd;

  private void Awake() {
    PlayerControls.OnMovement += HandleMovement;
    PlayerControls.OnSpaceBarPressed += HandleRoll;
    PlayerFootprint.OnSandpitEnter += SandpitSlow;
    PlayerFootprint.OnSandpitExit += ResetSpeed;

    PlayerHealth.OnNinjaIFramesEnabled += HandleNinjaIFramesEnabled;
    PlayerHealth.OnNinjaIFramesDisabled += HandleNinjaIFramesDisabled;

    _rb = gameObject.GetComponent<Rigidbody2D>();

    _lastMovementVector = Vector2.zero;

    LevelManager.OnDialogueStart += HandleDialogueChange;
		LevelManager.OnDialogueEnd += HandleDialogueChange;

    _currentMovementSpeed = _movementSpeed;
    OnSpeedChange?.Invoke(this, EventArgs.Empty);
  }

  private void ResetSpeed(object sender, EventArgs e) {
    _currentMovementSpeed = _movementSpeed;
    OnSpeedChange?.Invoke(this, EventArgs.Empty);
  }

  private void SandpitSlow(object sender, EventArgs e) {
    _currentMovementSpeed = _currentMovementSpeed * 0.5f;
    OnSpeedChange?.Invoke(this, EventArgs.Empty);
  }

  private void HandleNinjaIFramesEnabled(object sender, EventArgs e) {
    _currentMovementSpeed *= 1.5f;
    OnSpeedChange?.Invoke(this, EventArgs.Empty);
    _isVisible = false;
  }

  private void HandleNinjaIFramesDisabled(object sender, EventArgs e) {
    _currentMovementSpeed /= 1.5f;
    OnSpeedChange?.Invoke(this, EventArgs.Empty);
    _isVisible = true;
  }

  private void HandleMovement(object sender, MovementVectorEventArgs e) {
    if (_rb == null) {
      return;
    }

    if (Math.Abs(_currentMovementSpeed - _movementSpeed) > 0.01f) _rb.velocity = e.vector * _currentMovementSpeed;
    else _rb.velocity = e.vector * _movementSpeed;

    if (e.vector != Vector2.zero) {
      OnMovementBegin?.Invoke(this, EventArgs.Empty);
    } else {
      OnMovementEnd?.Invoke(this, EventArgs.Empty);
    }

    _lastMovementVector = e.vector;
  }

  private void HandleRoll(object sender, EventArgs e) {
    if (_canRoll && _isVisible) {
      StartCoroutine(Roll());
    }
  }

  IEnumerator Roll() {
    if (_rb == null) {
      yield break;
    }

    _rb.velocity = _lastMovementVector * _rollSpeed;
    _rb.drag = _rollDrag;

    Direction rollDirection = (_lastMovementVector.x >= 0) ? Direction.Right : Direction.Left;

    OnRollBegin?.Invoke(this, new RollEventArgs(_rollDuration, rollDirection));

    yield return new WaitForSeconds(_rollDuration);

    _rb.velocity = Vector2.zero;
    _rb.drag = 0;

    OnRollEnd?.Invoke(this, EventArgs.Empty);

    StartCoroutine(RollCooldown());
  }

  IEnumerator RollCooldown() {
    _canRoll = false;

    yield return new WaitForSeconds(_rollCooldown);

    _canRoll = true;
  }

  private void HandleDialogueChange(object sender, EventArgs e) {
    _rb.velocity = Vector2.zero;
  }

  public void IncreaseMoveSpeed(float increment) {
    _movementSpeed += increment;
    _currentMovementSpeed = _movementSpeed;
    OnSpeedChange?.Invoke(this, EventArgs.Empty);
  }

  public void MultiplyRollSpeed(float multiplyValue) {
    _rollSpeed *= multiplyValue;
  }

  public void MultiplyRollCooldown(float multiplyValue) {
    _rollCooldown *= multiplyValue;
  }

  private void OnDestroy() {
    PlayerControls.OnMovement -= HandleMovement;
    PlayerControls.OnSpaceBarPressed -= HandleRoll;
    LevelManager.OnDialogueStart -= HandleDialogueChange;
		LevelManager.OnDialogueEnd -= HandleDialogueChange;
    PlayerHealth.OnNinjaIFramesEnabled -= HandleNinjaIFramesEnabled;
    PlayerHealth.OnNinjaIFramesDisabled -= HandleNinjaIFramesDisabled;
  }
}
