using System;
using UnityEngine;

public class EggGuardHealth : EnemyHealth {
  public static event EventHandler<EnemyStatusEventArgs> OnEggGuardStatusDamage;

  public override void Damage(float amount) {
    _currentHealth -= amount;

    if (_currentHealth <= 0) {
      Die();
    }
  }

  public override void DamageWithStatus(float amount, StatusCondition status) {
    OnEggGuardStatusDamage?.Invoke(this, new EnemyStatusEventArgs(status));

    Damage(amount);
  }

  public override void DamageWithType(float amount, DamageType type) {
    float bonusDamage = (type == _weakTo) ? (amount * 0.5f) : 0;
    Damage(amount + bonusDamage);
  }

  protected override void Die() {
    Destroy(gameObject);
  }
}
