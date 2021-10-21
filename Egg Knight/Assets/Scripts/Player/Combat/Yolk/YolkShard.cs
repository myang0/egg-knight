using UnityEngine;

public class YolkShard : Projectile {
  protected override void Awake() {
    int randomAngle = Random.Range(0, 360);

    transform.eulerAngles = new Vector3(0, 0, randomAngle);

    base.Awake();
  }

  protected override void Despawn() {
    Destroy(gameObject);
  }

  protected override void OnTriggerEnter2D(Collider2D collider) {
    EnemyHealth enemyHealth = collider.gameObject.GetComponent<EnemyHealth>();
    
    if (enemyHealth != null) {
      enemyHealth.Damage(_damage);
    }

    StopCoroutine(DespawnTimer());
    Despawn();
  }
}
