using System;
using System.Collections.Generic;
using UnityEngine;

public class BroccoliHealth : EnemyHealth {
  public event EventHandler<EnemyStatusEventArgs> OnBroccoliStatusDamage;

  private Animator _anim;

  protected override void Awake() {
    _anim = GetComponent<Animator>();

    base.Awake();
  }

  public override void Damage(float amount) {
    if (_anim.GetBool("IsParrying")) {
      _anim.SetBool("HitDuringParry", true);
      
      return;
    }

    base.Damage(amount);
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