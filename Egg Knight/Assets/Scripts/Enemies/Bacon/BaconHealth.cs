using System;
using System.Collections.Generic;
using UnityEngine;

public class BaconHealth : EnemyHealth {
  public event EventHandler<EnemyStatusEventArgs> OnBaconStatusDamage;

  public override void DamageWithStatuses(float amount, List<StatusCondition> statuses) {
    OnBaconStatusDamage?.Invoke(this, new EnemyStatusEventArgs(statuses));

    Damage(amount); 
  }

  public override void DamageWithStatusesAndType(float amount, List<StatusCondition> statuses, DamageType type) {
    OnBaconStatusDamage?.Invoke(this, new EnemyStatusEventArgs(statuses));

    DamageWithType(amount, type);
  }
}
