using UnityEngine;

public class EggnaSlashSwoosh : BaseEnemyWeapon {
  [SerializeField] private float _attackRange;
  [SerializeField] private LayerMask _playerLayer;

  [SerializeField] private Transform _attackPoint;

  public void SetRotation(float angle) {
    transform.eulerAngles = new Vector3(0, 0, angle);
  }

  public override void EnableHitbox() {
    Collider2D[] playersInRange = Physics2D.OverlapCircleAll(_attackPoint.position, _attackRange, _playerLayer);

    if (playersInRange.Length > 0) {
      DamagePlayer(playersInRange[0]);
    }
  }

  public override void OnAnimationEnd() {
    Destroy(gameObject);
  }

  protected override void OnDrawGizmosSelected() {
    Gizmos.color = Color.red;

    if (_attackPoint != null) {
      Gizmos.DrawWireSphere(_attackPoint.position, _attackRange);
    }
  }
}
