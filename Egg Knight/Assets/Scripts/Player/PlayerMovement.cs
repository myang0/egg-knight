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

  public static event EventHandler<RollEventArgs> OnRollBegin;
  public static event EventHandler OnRollEnd;

  private void Awake() {
    PlayerControls.OnMovement += HandleMovement;
    PlayerControls.OnRightClick += HandleRoll;

    _rb = gameObject.GetComponent<Rigidbody2D>();
  }

  private void HandleMovement(object sender, MovementVectorEventArgs e) {
    _rb.velocity = e.vector * _movementSpeed;
  }

  private void HandleRoll(object sender, EventArgs e) {
    if (_canRoll) {
      StartCoroutine(Roll());
    }
  }

  IEnumerator Roll() {
    Vector3 mousePosInWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);

    Vector2 vectorToMouse = VectorHelper.GetVectorToPoint(transform.position, mousePosInWorld);

    _rb.velocity = vectorToMouse * _rollSpeed;
    _rb.drag = _rollDrag;

    Direction rollDirection = (vectorToMouse.x >= 0) ? Direction.Right : Direction.Left;

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
