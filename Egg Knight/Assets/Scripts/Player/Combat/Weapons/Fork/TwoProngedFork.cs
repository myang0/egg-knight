using UnityEngine;

public class TwoProngedFork : BasePlayerWeapon {
  [SerializeField] private Transform _attackPoint;
  [SerializeField] private float _attackWidth;
  [SerializeField] private float _attackHeight;

  [SerializeField] private LayerMask _enemyLayer;

  public override void EnableHitbox() {
    float hitboxAngle = transform.eulerAngles.z;

    Collider2D[] enemiesInRange = Physics2D.OverlapBoxAll(
      _attackPoint.position,
      new Vector2(_attackWidth, _attackHeight),
      hitboxAngle,
      _enemyLayer
    );

    DamageEnemies(enemiesInRange);
  }

  protected override void OnDrawGizmosSelected() {
    Gizmos.color = Color.red;
    Gizmos.DrawWireCube(_attackPoint.position, new Vector3(_attackWidth, _attackHeight, 1));
  }
}
