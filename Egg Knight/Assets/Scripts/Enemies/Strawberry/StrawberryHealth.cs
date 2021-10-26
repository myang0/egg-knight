using System;
using System.Collections.Generic;
using UnityEngine;

public class StrawberryHealth : EnemyHealth {
  public event EventHandler<EnemyStatusEventArgs> OnStrawberryStatusDamage;

  public override void DamageWithStatuses(float amount, List<StatusCondition> statuses) {
    OnStrawberryStatusDamage?.Invoke(this, new EnemyStatusEventArgs(statuses));

    Damage(amount);
  }

  public override void DamageWithStatusesAndType(float amount, List<StatusCondition> statuses, DamageType type) {
    OnStrawberryStatusDamage?.Invoke(this, new EnemyStatusEventArgs(statuses));

    DamageWithType(amount, type);
  }
}
