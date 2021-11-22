using System;
using System.Collections.Generic;
using UnityEngine;

public class SausageMinionHealth : EnemyHealth {
  public Animator _anim;

  public event EventHandler OnSausageMinionDeath;

  public event EventHandler<EnemyStatusEventArgs> OnSausageMinionStatusDamage;

  protected override void Awake() {
    _anim = GetComponent<Animator>();

    SausageHealth.OnSausageDeath += HandleBossDeath;
    base.Awake();
  }

  protected override void Die() {
    _anim.Play("Dead");

    OnSausageMinionDeath?.Invoke(this, EventArgs.Empty);
    base.Die();
  }

  private void HandleBossDeath(object sender, EventArgs e) {
    Die();
  }

  public override void DamageWithStatuses(float amount, List<StatusCondition> statuses) {
    OnSausageMinionStatusDamage?.Invoke(this, new EnemyStatusEventArgs(statuses));

    Damage(amount);
  }

  public override void DamageWithStatusesAndType(float amount, List<StatusCondition> statuses, DamageType type) {
    OnSausageMinionStatusDamage?.Invoke(this, new EnemyStatusEventArgs(statuses));

    DamageWithType(amount, type);
  }

  private void OnDestroy() {
    SausagePartyAttack.Partygoers--;
    SausageHealth.OnSausageDeath -= HandleBossDeath;
  }
}
