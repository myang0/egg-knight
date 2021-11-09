using System;
using System.Collections.Generic;
using UnityEngine;

public class CornHealth : EnemyHealth {
  public event EventHandler<EnemyStatusEventArgs> OnCornStatusDamage;

  public override void DamageWithStatuses(float amount, List<StatusCondition> statuses) {
    OnCornStatusDamage?.Invoke(this, new EnemyStatusEventArgs(statuses));

    Damage(amount);
  }

  public override void DamageWithStatusesAndType(float amount, List<StatusCondition> statuses, DamageType type) {
    OnCornStatusDamage?.Invoke(this, new EnemyStatusEventArgs(statuses));

    DamageWithType(amount, type);
  }
}
