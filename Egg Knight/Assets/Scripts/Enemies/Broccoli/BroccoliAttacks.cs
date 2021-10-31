using UnityEngine;

public class BroccoliAttacks : MonoBehaviour {
  [SerializeField] private Transform _leftWalkAttackPoint;
  [SerializeField] private Transform _rightWalkAttackPoint;

  [SerializeField] private float _walkAttackRange;
  [SerializeField] private float _walkAttackDamage;

  [SerializeField] private LayerMask _playerLayer;

  [SerializeField] private Transform _throwPivotPoint;
  [SerializeField] private Transform _leftThrowPoint;
  [SerializeField] private Transform _rightThrowPoint;

  [SerializeField] private Transform _spinAttackPoint;

  [SerializeField] private float _spinAttackWidth;
  [SerializeField] private float _spinAttackHeight;
  [SerializeField] private float _spinAttackDamage;

  [SerializeField] private GameObject _boomerangBladePrefab;

  [SerializeField] private GameObject _greatSlashPrefab;

  private void Awake() {
    _playerLayer = LayerMask.NameToLayer("Player");
  }

  public void LeftWalkAttack() {
    Collider2D[] playersInRange = GetPlayersInWalkAttackRange(_leftWalkAttackPoint);

    if (playersInRange.Length > 0) {
      DamagePlayer(playersInRange[0]);
    }
  }

  public void RightWalkAttack() {
    Collider2D[] playersInRange = GetPlayersInWalkAttackRange(_rightWalkAttackPoint);

    if (playersInRange.Length > 0) {
      DamagePlayer(playersInRange[0]);
    }
  }

  public void SpinAttack() {
    Collider2D[] playersInRange = Physics2D.OverlapBoxAll(
      _spinAttackPoint.position,
      new Vector2(_spinAttackWidth, _spinAttackHeight),
      0,
      _playerLayer
    );

    if (playersInRange.Length > 0) {
      DamagePlayer(playersInRange[0]);
    }
  }

  private Collider2D[] GetPlayersInWalkAttackRange(Transform attackPoint) {
    return Physics2D.OverlapCircleAll(attackPoint.position, _walkAttackRange, _playerLayer);
  }

  private void DamagePlayer(Collider2D player) {
    PlayerHealth playerHealth = player.gameObject.GetComponent<PlayerHealth>();
    playerHealth?.Damage(_walkAttackDamage);
  }

  public void ThrowBlades() {
    Transform playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    Vector2 vectorToPlayer = VectorHelper.GetVectorToPoint(transform.position, playerTransform.position);

    float angleToPlayer = Vector2.SignedAngle(Vector2.up, vectorToPlayer);
    _throwPivotPoint.eulerAngles = new Vector3(0, 0, angleToPlayer);

    if (_boomerangBladePrefab != null) {
      Instantiate(_boomerangBladePrefab, _leftThrowPoint.position, Quaternion.identity);
      Instantiate(_boomerangBladePrefab, _rightThrowPoint.position, Quaternion.identity);
    }
  }

  public void GreatSlash() {
    if (_greatSlashPrefab != null) {
      Instantiate(_greatSlashPrefab, transform.position, Quaternion.identity);
    }
  }

  private void OnDrawGizmosSelected() {
    Gizmos.color = Color.red;

    Gizmos.DrawWireSphere(_leftWalkAttackPoint.position, _walkAttackRange);
    Gizmos.DrawWireSphere(_rightWalkAttackPoint.position, _walkAttackRange);

    Gizmos.DrawWireCube(_spinAttackPoint.position, new Vector3(_spinAttackWidth, _spinAttackHeight, 1));
  }
}
