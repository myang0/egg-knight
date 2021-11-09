using UnityEngine;

public class ButterKnife : BasePlayerWeapon {
  [SerializeField] private Transform _attackPoint;
  [SerializeField] private float _attackRange;

  [SerializeField] private LayerMask _enemyLayer;
  [SerializeField] private LayerMask _obstacleLayer;

  public override void EnableHitbox() {
    Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(_attackPoint.position, _attackRange, _enemyLayer);
    Collider2D[] obstaclesInRange = 
      Physics2D.OverlapCircleAll(_attackPoint.position, _attackRange, _obstacleLayer);
    
    Collider2D[] enemiesHit = new Collider2D[enemiesInRange.Length + obstaclesInRange.Length];
    enemiesInRange.CopyTo(enemiesHit, 0);
    obstaclesInRange.CopyTo(enemiesHit, enemiesInRange.Length);
    
    Collider2D[] coinsInRange = Physics2D.OverlapCircleAll(_attackPoint.position, _attackRange, _coinLayer);

    DamageEnemies(enemiesHit);
    CollectCoins(coinsInRange);
  }

  protected override void OnDrawGizmosSelected() {
    Gizmos.color = Color.red;
    Gizmos.DrawWireSphere(_attackPoint.position, _attackRange);
  }
}
