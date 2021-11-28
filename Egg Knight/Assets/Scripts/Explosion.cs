using UnityEngine;

public class Explosion : MonoBehaviour {
  [SerializeField] protected float _explosionRange;

  [SerializeField] protected float _explosionDamage;

  [SerializeField] protected LayerMask _hitLayer;

  public virtual void OnExplode() {
    Collider2D[] entitiesInRange = Physics2D.OverlapCircleAll(transform.position, _explosionRange, _hitLayer);

    foreach (Collider2D entity in entitiesInRange) {
      Health eHealth = entity.GetComponent<Health>();

      eHealth?.Damage(_explosionDamage);
    }
  }

  public void OnExplosionEnd() {
    Destroy(gameObject);
  }

  private void OnDrawGizmosSelected() {
    Gizmos.color = Color.red;
    Gizmos.DrawWireSphere(transform.position, _explosionRange);
  }
}
