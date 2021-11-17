using System;
using System.Collections.Generic;
using UnityEngine;

public class RoyalEggHealth : EnemyHealth {
  public event EventHandler<EnemyStatusEventArgs> OnRoyalEggStatusDamage;

  protected override void Awake() {
    isInvulnerable = true;
    base.Awake();
  }

  public override void DamageWithStatuses(float amount, List<StatusCondition> statuses) {
    if (isInvulnerable) {
      Damage(0);
    } else {
      OnRoyalEggStatusDamage?.Invoke(this, new EnemyStatusEventArgs(statuses));

      Damage(amount);
    }
  }

  public override void DamageWithStatusesAndType(float amount, List<StatusCondition> statuses, DamageType type) {
    OnRoyalEggStatusDamage?.Invoke(this, new EnemyStatusEventArgs(statuses));

    DamageWithType(amount, type);
  }
}
