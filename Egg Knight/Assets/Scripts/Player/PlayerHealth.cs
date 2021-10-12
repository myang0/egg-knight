using System;
using UnityEngine;

public class PlayerHealth : Health {
  protected override void Awake() {
    PlayerMovement.OnRollBegin += DisableHitbox;
    PlayerMovement.OnRollEnd += EnableHitbox;
    
    base.Awake();
  }

  private void DisableHitbox(object sender, RollEventArgs e) {
    Physics2D.IgnoreLayerCollision((int)Layers.Player, (int)Layers.Enemy);
    Physics2D.IgnoreLayerCollision((int)Layers.Player, (int)Layers.EnemyProjectile);
    Physics2D.IgnoreLayerCollision((int)Layers.Player, (int)Layers.EnemyWeapon);
  }

  private void EnableHitbox(object sender, EventArgs e) {
    Physics2D.IgnoreLayerCollision((int)Layers.Player, (int)Layers.Enemy, false);
    Physics2D.IgnoreLayerCollision((int)Layers.Player, (int)Layers.EnemyProjectile, false);
    Physics2D.IgnoreLayerCollision((int)Layers.Player, (int)Layers.EnemyWeapon, false);
  }

  public override void Damage(float amount) {
    _currentHealth -= amount;

    if (_currentHealth <= 0) {
      Die();
    } 
  }
  
  protected override void Die() {
    Destroy(gameObject);
  }
}
