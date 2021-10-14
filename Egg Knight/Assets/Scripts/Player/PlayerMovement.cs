using System;
using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
  private Rigidbody2D _rb;

  [SerializeField] private float _movementSpeed = 5.0f;

  [SerializeField] private float _rollSpeed = 15.0f;
  [SerializeField] private float _rollDrag = 3.0f;
  [SerializeField] private float _rollDuration = 0.5f;

  [SerializeField] private float _rollCooldown = 0.25f;
  private bool _canRoll = true;

  private Vector2 _lastMovementVector;

  public static event EventHandler<RollEventArgs> OnRollBegin;
  public static event EventHandler OnRollEnd;

  private void Awake() {
    PlayerControls.OnMovement += HandleMovement;
    PlayerControls.OnSpaceBarPressed += HandleRoll;

    _rb = gameObject.GetComponent<Rigidbody2D>();

    _lastMovementVector = Vector2.zero;
  }

  private void HandleMovement(object sender, MovementVectorEventArgs e) {
    _rb.velocity = e.vector * _movementSpeed;

    if (e.vector.Equals(Vector2.zero) == false) {
      _lastMovementVector = e.vector;
    }
  }

  private void HandleRoll(object sender, EventArgs e) {
    if (_canRoll) {
      StartCoroutine(Roll());
    }
  }

  IEnumerator Roll() {
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
}
