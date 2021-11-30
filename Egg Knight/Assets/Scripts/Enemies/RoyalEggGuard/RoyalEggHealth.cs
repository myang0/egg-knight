using System;
using System.Collections.Generic;
using UnityEngine;

public class RoyalEggHealth : EnemyHealth {
  public event EventHandler<EnemyStatusEventArgs> OnRoyalEggStatusDamage;

  protected override void Awake() {
    isInvulnerable = true;

    EggnaHealth.OnEggnaDeath += HandleEggnaDeath;

    base.Awake();
  }

  private void HandleEggnaDeath(object sender, EventArgs e) {
    base.Die();
  }

  public override void DamageWithStatuses(float amount, List<StatusCondition> statuses) {
    if (isInvulnerable) {
      Damage(0);
    } else {
      OnRoyalEggStatusDamage?.Invoke(this, new EnemyStatusEventArgs(statuses));

      Damage(amount);
    }
  }

  public override bool DamageWithStatusesAndType(float amount, List<StatusCondition> statuses, DamageType type) {
    bool isDamageDealt = DamageWithType(amount, type);
    if (isDamageDealt) OnRoyalEggStatusDamage?.Invoke(this, new EnemyStatusEventArgs(statuses));

    return isDamageDealt;
  }

  private void OnDestroy() {
    EggnaHealth.OnEggnaDeath -= HandleEggnaDeath;
  }
}
