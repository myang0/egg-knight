using UnityEngine;

public class ButterKnife : BasePlayerWeapon {
  [SerializeField] private Transform _attackPoint;
  [SerializeField] private float _attackRange;

  public override void EnableHitbox() {
    Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(_attackPoint.position, _attackRange, _enemyLayer);
    Collider2D[] coinsInRange = Physics2D.OverlapCircleAll(_attackPoint.position, _attackRange, _coinLayer);

    DamageEnemies(enemiesInRange);
    CollectCoins(coinsInRange);
  }

  protected override void OnDrawGizmosSelected() {
    Gizmos.color = Color.red;
    Gizmos.DrawWireSphere(_attackPoint.position, _attackRange);
  }
}
