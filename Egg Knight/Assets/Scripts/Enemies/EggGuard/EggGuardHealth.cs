using System;
using System.Collections.Generic;
using UnityEngine;

public class EggGuardHealth : EnemyHealth {
  public event EventHandler<EnemyStatusEventArgs> OnEggGuardStatusDamage;

  public override void DamageWithStatuses(float amount, List<StatusCondition> statuses) {
    OnEggGuardStatusDamage?.Invoke(this, new EnemyStatusEventArgs(statuses));

    Damage(amount);
  }

  public override bool DamageWithStatusesAndType(float amount, List<StatusCondition> statuses, DamageType type) {
    bool isDamageDealt = DamageWithType(amount, type);
    if (isDamageDealt) OnEggGuardStatusDamage?.Invoke(this, new EnemyStatusEventArgs(statuses));

    return isDamageDealt;
  }
}
