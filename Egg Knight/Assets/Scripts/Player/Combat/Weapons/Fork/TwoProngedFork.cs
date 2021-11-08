using UnityEngine;

public class TwoProngedFork : BasePlayerWeapon {
  [SerializeField] private Transform _attackPoint;
  [SerializeField] private float _attackWidth;
  [SerializeField] private float _attackHeight;

  public override void EnableHitbox() {
    float hitboxAngle = transform.eulerAngles.z;

    Collider2D[] enemiesInRange = Physics2D.OverlapBoxAll(
      _attackPoint.position,
      new Vector2(_attackWidth, _attackHeight),
      hitboxAngle,
      _enemyLayer
    );

    Collider2D[] coinsInRange = Physics2D.OverlapBoxAll(
      _attackPoint.position,
      new Vector2(_attackWidth, _attackHeight),
      hitboxAngle,
      _coinLayer
    );

    DamageEnemies(enemiesInRange);
    CollectCoins(coinsInRange);
  }

  protected override void OnDrawGizmosSelected() {
    Gizmos.color = Color.red;
    Gizmos.DrawWireCube(_attackPoint.position, new Vector3(_attackWidth, _attackHeight, 1));
  }
}
