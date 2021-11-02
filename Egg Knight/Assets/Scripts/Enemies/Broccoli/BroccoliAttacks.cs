using System.Collections.Generic;
using System.Linq;
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

  [SerializeField] private Transform _leftSpinAttackPoint;
  [SerializeField] private Transform _middleSpinAttackPoint;
  [SerializeField] private Transform _rightSpinAttackPoint;

  [SerializeField] private float _spinAttackDamage;
  [SerializeField] private float _middleSpinAttackRange;
  [SerializeField] private float _sideSpinAttackRange;

  [SerializeField] private GameObject _boomerangBladePrefab;

  [SerializeField] private GameObject _greatSlashPrefab;

  private Transform _playerTransform;

  private void Awake() {
    _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
  }

  public void LeftWalkAttack() {
    Transform attackPoint = IsSpriteFlipped() ? _rightWalkAttackPoint : _leftWalkAttackPoint;
    Collider2D[] playersInRange = GetPlayersInRange(attackPoint, _walkAttackRange);

    if (playersInRange.Length > 0) {
      DamagePlayer(playersInRange[0], _walkAttackDamage);
    }
  }

  public void RightWalkAttack() {
    Transform attackPoint = IsSpriteFlipped() ? _leftWalkAttackPoint : _rightWalkAttackPoint;
    Collider2D[] playersInRange = GetPlayersInRange(attackPoint, _walkAttackRange);

    if (playersInRange.Length > 0) {
      DamagePlayer(playersInRange[0], _walkAttackDamage);
    }
  }

  private bool IsSpriteFlipped() {
    return _playerTransform.position.x < transform.position.x;
  }

  public void SpinAttack() {
    List<Collider2D> playersInLeftRange = GetPlayersInRange(_leftSpinAttackPoint, _sideSpinAttackRange).ToList();
    List<Collider2D> playersInMiddleRange = GetPlayersInRange(_middleSpinAttackPoint, _middleSpinAttackRange).ToList();
    List<Collider2D> playersInRightRange = GetPlayersInRange(_rightSpinAttackPoint, _sideSpinAttackRange).ToList();

    Collider2D[] playersInRange = playersInLeftRange
      .Union(playersInMiddleRange)
      .Union(playersInRightRange)
      .ToArray();

    if (playersInRange.Length > 0) {
      DamagePlayer(playersInRange[0], _spinAttackDamage);
    }
  }

  private Collider2D[] GetPlayersInRange(Transform attackPoint, float attackRange) {
    return Physics2D.OverlapCircleAll(attackPoint.position, attackRange, _playerLayer);
  }

  private void DamagePlayer(Collider2D player, float damage) {
    PlayerHealth playerHealth = player.gameObject.GetComponent<PlayerHealth>();
    playerHealth?.Damage(damage);
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

    Gizmos.DrawWireSphere(_leftSpinAttackPoint.position, _sideSpinAttackRange);
    Gizmos.DrawWireSphere(_middleSpinAttackPoint.position, _middleSpinAttackRange);
    Gizmos.DrawWireSphere(_rightSpinAttackPoint.position, _sideSpinAttackRange);
  }
}
