using System;
using System.Collections.Generic;
using UnityEngine;

public class PeaHealth : EnemyHealth {
  public event EventHandler<EnemyStatusEventArgs> OnPeaStatusDamage;

  public override void DamageWithStatuses(float amount, List<StatusCondition> statuses) {
    OnPeaStatusDamage?.Invoke(this, new EnemyStatusEventArgs(statuses));

    Damage(amount);
  }

  public override bool DamageWithStatusesAndType(float amount, List<StatusCondition> statuses, DamageType type) {
    bool isDamageDealt = DamageWithType(amount, type);
    if (isDamageDealt) OnPeaStatusDamage?.Invoke(this, new EnemyStatusEventArgs(statuses));

    return isDamageDealt;
  }
}
