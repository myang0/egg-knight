using System.Collections.Generic;
using UnityEngine;

public class YolkProjectile : Projectile {
  protected override void Despawn() {
    Destroy(gameObject);
  }

  protected override void OnTriggerEnter2D(Collider2D collider) {
    EnemyHealth enemyHealth = collider.gameObject.GetComponent<EnemyHealth>();
    
    if (enemyHealth != null) {
      enemyHealth.DamageWithStatuses(_damage, new List<StatusCondition>() { StatusCondition.Yolked });
    }

    StopCoroutine(DespawnTimer());
    Despawn();
  }
}
