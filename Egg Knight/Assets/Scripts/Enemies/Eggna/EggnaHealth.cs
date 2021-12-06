using System;
using System.Collections.Generic;
using UnityEngine;

public class EggnaHealth : EnemyHealth {
  public event EventHandler<EnemyStatusEventArgs> OnEggnaStatusDamage;

  public static event EventHandler<HealthChangeEventArgs> OnEggnaDamage;
  public static event EventHandler OnEggnaDeath;

  public static event EventHandler OnEggnaBelowHalfHealth;

  private Animator _anim;

  private bool _aboveHalfHealth = true;

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

    if (_aboveHalfHealth && CurrentHealthPercentage() <= 0.5f) {
      Fungus.Flowchart.BroadcastFungusMessage("LadyEggnaHalf");

      _aboveHalfHealth = false;
    }
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
