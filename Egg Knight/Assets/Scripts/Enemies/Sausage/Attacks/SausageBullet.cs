using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SausageBullet : Projectile {
  protected override void Awake() {
    SausageHealth.OnSausageDeath += HandleSausageDeath;

    base.Awake();
  }

  private void HandleSausageDeath(object sender, EventArgs e) {
    Despawn();
  }

  protected override void Despawn() {
    Destroy(gameObject);
  }

  protected override void OnTriggerEnter2D(Collider2D collider) {
    PlayerHealth playerHealth = collider.gameObject.GetComponent<PlayerHealth>();
    
    if (playerHealth != null) {
      playerHealth.Damage(_damage);
    }

    if (collider.gameObject.layer == LayerMask.NameToLayer("Obstacle") || playerHealth != null) {
      if (collider.GetComponent<CactusHealth>() != null) return;
      StopCoroutine(DespawnTimer());
      Despawn();
    }
  }

  private void OnDestroy() {
    SausageHealth.OnSausageDeath -= HandleSausageDeath;
  }
}