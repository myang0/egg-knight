using UnityEngine;

public class KnifeBeam : Projectile {
  [SerializeField] private GameObject _particles;

  private void FixedUpdate() {
    transform.Rotate(0, 10, 0);
  }

  protected override void Despawn() {
    Instantiate(_particles, transform.position, Quaternion.identity);
    Destroy(gameObject);
  }

  protected override void OnTriggerEnter2D(Collider2D collider) {
    EnemyHealth enemyHealth = collider.gameObject.GetComponent<EnemyHealth>();
    
    if (enemyHealth != null && collider.gameObject.layer == LayerMask.NameToLayer("Enemy")) {
      enemyHealth.Damage(_damage);
    }

    if (collider.gameObject.layer == LayerMask.NameToLayer("Obstacle") || enemyHealth != null) {
      StopCoroutine(DespawnTimer());
      Despawn();
    }
  }
}
