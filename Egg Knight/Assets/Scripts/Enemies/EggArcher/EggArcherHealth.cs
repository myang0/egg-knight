using System;
using System.Collections.Generic;
using UnityEngine;

public class EggArcherHealth : EnemyHealth {
  public event EventHandler<EnemyStatusEventArgs> OnEggArcherStatusDamage;

  public override void DamageWithStatuses(float amount, List<StatusCondition> statuses) {
    if (isInvulnerable == false) {
      OnEggArcherStatusDamage?.Invoke(this, new EnemyStatusEventArgs(statuses));

      Damage(amount); 
    }
  }

  public override void DamageWithStatusesAndType(float amount, List<StatusCondition> statuses, DamageType type) {
    if (isInvulnerable == false) {
      OnEggArcherStatusDamage?.Invoke(this, new EnemyStatusEventArgs(statuses));

      DamageWithType(amount, type);
    }
  }
}
