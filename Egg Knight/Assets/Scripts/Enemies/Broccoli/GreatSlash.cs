using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GreatSlash : BaseEnemyWeapon {
  [SerializeField] private Transform _leftAttackPoint;
  [SerializeField] private Transform _rightAttackPoint;

  [SerializeField] private float _attackRange;

  [SerializeField] private LayerMask _playerLayer;

  public override void EnableHitbox() {
    List<Collider2D> playersInLeftRange = GetPlayersInAttackRange(_leftAttackPoint).ToList();
    List<Collider2D> playersInRightRange = GetPlayersInAttackRange(_rightAttackPoint).ToList();

    Collider2D[] playersInRange = playersInLeftRange.Union(playersInRightRange).ToArray();

    if (playersInRange.Length > 0) {
      DamagePlayer(playersInRange[0]);
    }
  }

  private Collider2D[] GetPlayersInAttackRange(Transform attackPoint) {
    return Physics2D.OverlapCircleAll(attackPoint.position, _attackRange, _playerLayer);
  }

  public override void OnAnimationEnd() {
    Destroy(gameObject);
  }

  protected override void OnDrawGizmosSelected() {
    Gizmos.color = Color.red;

    Gizmos.DrawWireSphere(_leftAttackPoint.position, _attackRange);
    Gizmos.DrawWireSphere(_rightAttackPoint.position, _attackRange);
  }
}
