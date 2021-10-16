using System;
using System.Collections.Generic;
using UnityEngine;

public class EggGuardHealth : EnemyHealth {
  public event EventHandler<EnemyStatusEventArgs> OnEggGuardStatusDamage;

  public override void DamageWithStatuses(float amount, List<StatusCondition> statuses) {
    OnEggGuardStatusDamage?.Invoke(this, new EnemyStatusEventArgs(statuses));

    Damage(amount);
  }

  public override void DamageWithStatusesAndType(float amount, List<StatusCondition> statuses, DamageType type) {
    OnEggGuardStatusDamage?.Invoke(this, new EnemyStatusEventArgs(statuses));

    DamageWithType(amount, type);
  }
}
