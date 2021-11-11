using System;
using System.Collections.Generic;
using UnityEngine;

public class TomatoHealth : EnemyHealth {
  public event EventHandler<EnemyStatusEventArgs> OnTomatoStatusDamage;

  public override void DamageWithStatuses(float amount, List<StatusCondition> statuses) {
    OnTomatoStatusDamage?.Invoke(this, new EnemyStatusEventArgs(statuses));

    Damage(amount);
  }

  public override void DamageWithStatusesAndType(float amount, List<StatusCondition> statuses, DamageType type) {
    OnTomatoStatusDamage?.Invoke(this, new EnemyStatusEventArgs(statuses));

    DamageWithType(amount, type);
  }

  public void OnExplode() {
    Die();
  }
}
