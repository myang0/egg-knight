using UnityEngine;

public class GreaseOrb : Projectile {
  protected override void Despawn() {
    Destroy(gameObject);
  }

  protected override void OnTriggerEnter2D(Collider2D collider) {
    PlayerHealth playerHealth = collider.gameObject.GetComponent<PlayerHealth>();
    
    if (playerHealth != null) {
      playerHealth.Damage(_damage);
    }
  }
}
