using System;
using System.Collections.Generic;
using UnityEngine;

public class BroccoliHealth : EnemyHealth {
  public event EventHandler<EnemyStatusEventArgs> OnBroccoliStatusDamage;

  public override void DamageWithStatuses(float amount, List<StatusCondition> statuses) {
    OnBroccoliStatusDamage?.Invoke(this, new EnemyStatusEventArgs(statuses));

    Damage(amount);
  }

  public override void DamageWithStatusesAndType(float amount, List<StatusCondition> statuses, DamageType type) {
    OnBroccoliStatusDamage?.Invoke(this, new EnemyStatusEventArgs(statuses));

    DamageWithType(amount, type);
  }
}