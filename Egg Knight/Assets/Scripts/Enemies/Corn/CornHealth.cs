using System;
using System.Collections.Generic;
using UnityEngine;

public class CornHealth : EnemyHealth {
  public event EventHandler<EnemyStatusEventArgs> OnCornStatusDamage;

  public override void DamageWithStatuses(float amount, List<StatusCondition> statuses) {
    OnCornStatusDamage?.Invoke(this, new EnemyStatusEventArgs(statuses));

    Damage(amount);
  }

  public override bool DamageWithStatusesAndType(float amount, List<StatusCondition> statuses, DamageType type) {
    bool isDamageDealt = DamageWithType(amount, type);
    if (isDamageDealt) OnCornStatusDamage?.Invoke(this, new EnemyStatusEventArgs(statuses));

    return isDamageDealt;
  }
}
