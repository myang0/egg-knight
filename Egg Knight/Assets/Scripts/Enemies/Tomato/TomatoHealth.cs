using System;
using System.Collections.Generic;
using UnityEngine;

public class TomatoHealth : EnemyHealth {
  public event EventHandler<EnemyStatusEventArgs> OnTomatoStatusDamage;

  public override void DamageWithStatuses(float amount, List<StatusCondition> statuses) {
    OnTomatoStatusDamage?.Invoke(this, new EnemyStatusEventArgs(statuses));

    Damage(amount);
  }

  public override bool DamageWithStatusesAndType(float amount, List<StatusCondition> statuses, DamageType type) {
    bool isDamageDealt = DamageWithType(amount, type);
    if (isDamageDealt) OnTomatoStatusDamage?.Invoke(this, new EnemyStatusEventArgs(statuses));

    return isDamageDealt;
  }

  public void OnExplode() {
    Die();
  }
}
