using UnityEngine;

public class YolkProjectile : Projectile {
  protected override void Despawn() {
    Destroy(gameObject);
  }

  protected override void OnTriggerEnter2D(Collider2D collider) {
    EnemyHealth enemyHealth = collider.gameObject.GetComponent<EnemyHealth>();
    
    if (enemyHealth != null) {
      enemyHealth.DamageWithStatus(_damage, StatusCondition.Yolked);
    }

    StopCoroutine(DespawnTimer());
    Despawn();
  }
}
