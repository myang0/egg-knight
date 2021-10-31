using UnityEngine;

public class BroccoliMovement : MonoBehaviour {
  [SerializeField] private float _longDistanceThreshold;
  [SerializeField] private float _shortDistanceThreshold;

  [SerializeField] private float _minSpinMoveSpeed;
  [SerializeField] private float _maxSpinMoveSpeed;

  [SerializeField] private float _walkMoveSpeed;

  private Transform _playerTransform;

  private EnemyBehaviour _eBehaviour;

  private Animator _anim;
  private Rigidbody2D _rb;

  private void Awake() {
    _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

    _eBehaviour = GetComponent<EnemyBehaviour>();

    _anim = GetComponent<Animator>();
    _rb = GetComponent<Rigidbody2D>();
  }

  private void FixedUpdate() {
    if (_anim.GetBool("IsWalking")) {
      WalkingMovement();
      return;
    }

    if (_anim.GetBool("IsSpinning")) {
      SpinningMovement();
      return;
    }
  }

  private void WalkingMovement() {
    _rb.velocity = VectorToPlayer() * _walkMoveSpeed;
  }

  private void SpinningMovement() {
    float distanceToPlayer = _eBehaviour.GetDistanceToPlayer();

    float clampedDistance = Mathf.Clamp(distanceToPlayer, _shortDistanceThreshold, _longDistanceThreshold);

    float distanceRange = _longDistanceThreshold - _shortDistanceThreshold;
    float speedRange = _maxSpinMoveSpeed - _minSpinMoveSpeed;

    float spinMoveSpeed = (((clampedDistance - _shortDistanceThreshold) / distanceRange) * speedRange) + _minSpinMoveSpeed;

    _rb.velocity = VectorToPlayer() * spinMoveSpeed;
  }

  private Vector2 VectorToPlayer() {
    return VectorHelper.GetVectorToPoint(transform.position, _playerTransform.position);
  }
}
