using UnityEngine;

public class EggGuardHealth : EnemyHealth {
  public override void Damage(float amount) {
    _currentHealth -= amount;

    if (_currentHealth <= 0) {
      Die();
    }
  } 

  public override void Damage(float amount, DamageType type) {
    float bonusDamage = (type == _weakTo) ? (amount * 0.5f) : 0;
    _currentHealth -= (amount + bonusDamage);

    if (_currentHealth <= 0) {
      Die();
    }
  }

  protected override void Die() {
    Destroy(gameObject);
  }
}
