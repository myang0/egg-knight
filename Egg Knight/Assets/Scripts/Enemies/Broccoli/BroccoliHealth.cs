using System;
using System.Collections.Generic;
using UnityEngine;

public class BroccoliHealth : EnemyHealth {
  public event EventHandler<EnemyStatusEventArgs> OnBroccoliStatusDamage;

  public static event EventHandler<HealthChangeEventArgs> OnBroccoliDamage;
  public static event EventHandler OnBroccoliDeath;

  private Animator _anim;
  private SoundPlayer _soundPlayer;

  [SerializeField] private AudioClip _parryStartClip;
  [SerializeField] private AudioClip _parryHitClip;

  private bool _parrySoundPlayed = false;

  protected override void Awake() {
    _anim = GetComponent<Animator>();
    _soundPlayer = GetComponent<SoundPlayer>();

    BroccoliStateManager bStateManager = GetComponent<BroccoliStateManager>();
    bStateManager.OnIdleEnd += HandleIdleEnd;

    base.Awake();
  }

  private void HandleIdleEnd(object sender, EventArgs e) {
    _parrySoundPlayed = false;
  }

  public void ParryStartSound() {
    if (_parrySoundPlayed == false) {
      _soundPlayer.PlayClip(_parryStartClip);

      _parrySoundPlayed = true;
    }
  }

  public override void Damage(float amount) {
    if (_anim.GetBool("IsActive") == false) {
      return;
    }

    if (_anim.GetBool("IsParrying")) {
      _soundPlayer.PlayClip(_parryHitClip);

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

  public override bool DamageWithStatusesAndType(float amount, List<StatusCondition> statuses, DamageType type) {
    bool isDamageDealt = DamageWithType(amount, type);
    if (isDamageDealt) OnBroccoliStatusDamage?.Invoke(this, new EnemyStatusEventArgs(statuses));

    return isDamageDealt;
  }
}