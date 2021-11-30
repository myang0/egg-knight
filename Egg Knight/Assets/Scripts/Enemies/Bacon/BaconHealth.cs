using System;
using System.Collections.Generic;
using UnityEngine;

public class BaconHealth : EnemyHealth {
  public static event EventHandler OnBaconDeath;

  public event EventHandler<EnemyStatusEventArgs> OnBaconStatusDamage;

  protected override void Awake() {
    EggnaHealth.OnEggnaDeath += HandleEggnaDeath;

    base.Awake();
  }

  private void HandleEggnaDeath(object sender, EventArgs e) {
    base.Die();
  }

  public override void DamageWithStatuses(float amount, List<StatusCondition> statuses) {
    OnBaconStatusDamage?.Invoke(this, new EnemyStatusEventArgs(statuses));

    Damage(amount); 
  }

  public override bool DamageWithStatusesAndType(float amount, List<StatusCondition> statuses, DamageType type) {
    bool isDamageDealt = DamageWithType(amount, type);
    if (isDamageDealt) OnBaconStatusDamage?.Invoke(this, new EnemyStatusEventArgs(statuses));

    return isDamageDealt;
  }

  protected override void Die() {
    OnBaconDeath?.Invoke(this, EventArgs.Empty);
    base.Die();
  }

  private void OnDestroy() {
    EggnaHealth.OnEggnaDeath -= HandleEggnaDeath;
  }
}
