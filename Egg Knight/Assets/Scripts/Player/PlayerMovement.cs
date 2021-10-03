using UnityEngine;

public class PlayerMovement : MonoBehaviour {
  private Rigidbody2D _rb;

  private Vector2 _lastVelocity;

  [SerializeField] private float _speed = 5.0f;

  private void Awake() {
    PlayerControls.OnMovement += HandleMovement;

    _rb = gameObject.GetComponent<Rigidbody2D>();
  }

  private void HandleMovement(object sender, MovementVectorEventArgs e) {
    _rb.velocity = e.vector * _speed;
  }
}