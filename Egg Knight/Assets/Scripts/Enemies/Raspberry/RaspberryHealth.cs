using System;
using System.Collections.Generic;
using UnityEngine;

public class RaspberryHealth : EnemyHealth {
  public event EventHandler<EnemyStatusEventArgs> OnRaspberryStatusDamage;

  public override void DamageWithStatuses(float amount, List<StatusCondition> statuses) {
    OnRaspberryStatusDamage?.Invoke(this, new EnemyStatusEventArgs(statuses));

    Damage(amount);
  }

  public override void DamageWithStatusesAndType(float amount, List<StatusCondition> statuses, DamageType type) {
    OnRaspberryStatusDamage?.Invoke(this, new EnemyStatusEventArgs(statuses));

    DamageWithType(amount, type);
  }
}
