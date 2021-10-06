using UnityEngine;

public class PlayerHealth : Health {
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