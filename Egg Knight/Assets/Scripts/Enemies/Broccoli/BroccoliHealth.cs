using System;
using System.Collections.Generic;
using UnityEngine;

public class BroccoliHealth : EnemyHealth {
  public event EventHandler<EnemyStatusEventArgs> OnBroccoliStatusDamage;

  public static event EventHandler<HealthChangeEventArgs> OnBroccoliDamage;
  public static event EventHandler OnBroccoliDeath;

  private Animator _anim;

  protected override void Awake() {
    _anim = GetComponent<Animator>();

    base.Awake();
  }

  public override void Damage(float amount) {
    if (_anim.GetBool("IsActive") == false) {
      return;
    }

    if (_anim.GetBool("IsParrying")) {
      _anim.SetBool("HitDuringParry", true);
      
      return;
    }

    OnBroccoliDamage?.Invoke(this, new HealthChangeEventArgs(CurrentHealthPercentage()));
    base.Damage(amount);
  }

  protected override void Die() {
    OnBroccoliDeath?.Invoke(this, EventArgs.Empty);
    Fungus.Flowchart.BroadcastFungusMessage("BrigandBroccoliEnd");
    _anim.Play("Dead");
    base.Die();
  }

  public override void DamageWithStatuses(float amount, List<StatusCondition> statuses) {
    OnBroccoliStatusDamage?.Invoke(this, new EnemyStatusEventArgs(statuses));

    Damage(amount);
  }

  public override void DamageWithStatusesAndType(float amount, List<StatusCondition> statuses, DamageType type) {
    OnBroccoliStatusDamage?.Invoke(this, new EnemyStatusEventArgs(statuses));

    DamageWithType(amount, type);
  }
}