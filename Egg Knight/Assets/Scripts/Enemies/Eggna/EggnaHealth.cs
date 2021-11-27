using System;
using System.Collections.Generic;
using UnityEngine;

public class EggnaHealth : EnemyHealth {
  public event EventHandler<EnemyStatusEventArgs> OnEggnaStatusDamage;

  public static event EventHandler<HealthChangeEventArgs> OnEggnaDamage;
  public static event EventHandler OnEggnaDeath;

  private Animator _anim;

  protected override void Awake() {
    _anim = GetComponent<Animator>();

    base.Awake();
  }

  public override void Damage(float amount) {
    if (_anim.GetBool("IsActive") == false) {
      return;
    }

    OnEggnaDamage?.Invoke(this, new HealthChangeEventArgs(CurrentHealthPercentage()));
    base.Damage(amount);
  }

  protected override void Die() {
    OnEggnaDeath?.Invoke(this, EventArgs.Empty);
    Fungus.Flowchart.BroadcastFungusMessage("LadyEggnaEnd");
    _anim.Play("Dead");
    base.Die();
  }

  public override void DamageWithStatuses(float amount, List<StatusCondition> statuses) {
    OnEggnaStatusDamage?.Invoke(this, new EnemyStatusEventArgs(statuses));

    Damage(amount);
  }

  public override bool DamageWithStatusesAndType(float amount, List<StatusCondition> statuses, DamageType type) {
    bool isDamageDealt = DamageWithType(amount, type);
    if (isDamageDealt) OnEggnaStatusDamage?.Invoke(this, new EnemyStatusEventArgs(statuses));

    return isDamageDealt;
  }
}
