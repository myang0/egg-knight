using System;
using System.Collections.Generic;
using UnityEngine;

public class SausageHealth : EnemyHealth {
  public event EventHandler<EnemyStatusEventArgs> OnSausageStatusDamage;

  public static event EventHandler<HealthChangeEventArgs> OnSausageDamage;
  public static event EventHandler OnSausageDeath;

  private Animator _anim;

  protected override void Awake() {
    _anim = GetComponent<Animator>();

    base.Awake();
  }

  public override void Damage(float amount) {
    OnSausageDamage?.Invoke(this, new HealthChangeEventArgs(CurrentHealthPercentage()));
    base.Damage(amount);
  }

  protected override void Die() {
    OnSausageDeath?.Invoke(this, EventArgs.Empty);
    Fungus.Flowchart.BroadcastFungusMessage("SheriffSausageEnd");
    _anim.Play("Dead");
    base.Die();
  }

  public override void DamageWithStatuses(float amount, List<StatusCondition> statuses) {
    OnSausageStatusDamage?.Invoke(this, new EnemyStatusEventArgs(statuses));

    Damage(amount);
  }

  public override void DamageWithStatusesAndType(float amount, List<StatusCondition> statuses, DamageType type) {
    OnSausageStatusDamage?.Invoke(this, new EnemyStatusEventArgs(statuses));

    DamageWithType(amount, type);
  }
}
