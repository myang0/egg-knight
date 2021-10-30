using System;
using Stage;
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

  public static event EventHandler OnMovementBegin;
  public static event EventHandler OnMovementEnd;

  public static event EventHandler<RollEventArgs> OnRollBegin;
  public static event EventHandler OnRollEnd;

  private void Awake() {
    PlayerControls.OnMovement += HandleMovement;
    PlayerControls.OnSpaceBarPressed += HandleRoll;

    _rb = gameObject.GetComponent<Rigidbody2D>();

    _lastMovementVector = Vector2.zero;

    LevelManager.OnDialogueStart += (object sender, EventArgs e) => _rb.velocity = Vector2.zero;
		LevelManager.OnDialogueEnd += (object sender, EventArgs e) => _rb.velocity = Vector2.zero;
  }

  private void HandleMovement(object sender, MovementVectorEventArgs e) {
    _rb.velocity = e.vector * _movementSpeed;

    if (e.vector != Vector2.zero) {
      OnMovementBegin?.Invoke(this, EventArgs.Empty);
    } else {
      OnMovementEnd?.Invoke(this, EventArgs.Empty);
    }

    _lastMovementVector = e.vector;
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

  public void MultiplyMoveSpeed(float multiplyValue) {
    _movementSpeed *= multiplyValue;
  }

  public void MultiplyRollSpeed(float multiplyValue) {
    _rollSpeed *= multiplyValue;
  }

  public void MultiplyRollCooldown(float multiplyValue) {
    _rollCooldown *= multiplyValue;
  }
  
  // private void OnCollisionEnter2D(Collision2D other) {
  //   if (other.collider.CompareTag("Enemy")) {
  //     if (other.collider.GetComponent<EnemyBehaviour>().isDead) {
  //       Physics2D.IgnoreCollision(other.collider, GetComponent<Collider2D>());
  //     }
  //   }
  // }
  //
  // private void OnCollisionStay2D(Collision2D other) {
  //   if (other.collider.CompareTag("Enemy")) {
  //     if (other.collider.GetComponent<EnemyBehaviour>().isDead) {
  //       Debug.Log("DEAD");
  //       Physics2D.IgnoreCollision(other.collider, GetComponent<Collider2D>());
  //     }
  //     else {
  //       Debug.Log("NOT DEAD");
  //     }
  //   }
  // }
}
