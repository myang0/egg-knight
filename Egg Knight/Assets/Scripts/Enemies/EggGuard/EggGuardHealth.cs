using System;
using UnityEngine;

public class EggGuardHealth : EnemyHealth {
  public static event EventHandler<EnemyStatusEventArgs> OnEggGuardStatusDamage;

  public override void DamageWithStatus(float amount, StatusCondition status) {
    OnEggGuardStatusDamage?.Invoke(this, new EnemyStatusEventArgs(status));

    Damage(amount);
  }
}
