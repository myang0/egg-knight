using UnityEngine;

public class Explosion : MonoBehaviour {
  [SerializeField] private Transform _explosionPoint;
  [SerializeField] private float _explosionRange;

  [SerializeField] private float _explosionDamage;

  [SerializeField] private LayerMask _hitLayer;

  public void OnExplode() {
    Collider2D[] entitiesInRange = Physics2D.OverlapCircleAll(_explosionPoint.position, _explosionRange, _hitLayer);

    foreach (Collider2D entity in entitiesInRange) {
      Health eHealth = entity.GetComponent<Health>();

      eHealth?.Damage(_explosionDamage);
    }
  }

  public void OnExplosionEnd() {
    Destroy(gameObject);
  }

  private void OnDrawGizmosSelected() {
    if (_explosionPoint != null) {
      Gizmos.color = Color.red;
      Gizmos.DrawWireSphere(_explosionPoint.position, _explosionRange);
    }
  }
}
