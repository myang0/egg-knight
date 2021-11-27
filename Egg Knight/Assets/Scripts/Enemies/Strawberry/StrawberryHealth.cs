using System;
using System.Collections.Generic;
using UnityEngine;

public class StrawberryHealth : EnemyHealth {
  public event EventHandler<EnemyStatusEventArgs> OnStrawberryStatusDamage;

  public override void DamageWithStatuses(float amount, List<StatusCondition> statuses) {
    OnStrawberryStatusDamage?.Invoke(this, new EnemyStatusEventArgs(statuses));

    Damage(amount);
  }

  public override bool DamageWithStatusesAndType(float amount, List<StatusCondition> statuses, DamageType type) {
    bool isDamageDealt = DamageWithType(amount, type);
    if (isDamageDealt) OnStrawberryStatusDamage?.Invoke(this, new EnemyStatusEventArgs(statuses));

    return isDamageDealt;
  }
}
