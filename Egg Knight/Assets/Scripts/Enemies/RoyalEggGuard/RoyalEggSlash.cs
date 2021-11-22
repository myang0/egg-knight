using UnityEngine;

public class RoyalEggSlash : BaseEnemyWeapon {
  [SerializeField] private float _attackRange;
  [SerializeField] private LayerMask _playerLayer;

  private SpriteRenderer _sr;

  public void SetRotation(float angle) {
    transform.eulerAngles = new Vector3(0, 0, angle);
  }

  public override void EnableHitbox() {
    Collider2D[] playersInRange = Physics2D.OverlapCircleAll(transform.position, _attackRange, _playerLayer);

    if (playersInRange.Length > 0) {
      DamagePlayer(playersInRange[0]);
    }
  }

  public override void OnAnimationEnd() {
    Destroy(gameObject);
  }

  protected override void OnDrawGizmosSelected() {
    Gizmos.color = Color.red;

    Gizmos.DrawWireSphere(transform.position, _attackRange);
  }
}
