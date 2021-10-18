using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyHealth : Health {
  [SerializeField] protected DamageType _weakTo;

  protected bool _isFrosted = false;

  protected override void Awake() {
    EnemyBehaviour enemyBehaviour = gameObject.GetComponent<EnemyBehaviour>();

    enemyBehaviour.OnFrosted += HandleFrosted;
    enemyBehaviour.OnIgnited += HandleIgnited;
    enemyBehaviour.OnElectrocuted += HandleElectrocuted;

    base.Awake();
  }

  protected virtual void HandleFrosted(object sender, EventArgs e) {
    StartCoroutine(Frost());
  }

  protected virtual IEnumerator Frost() {
    yield return new WaitForSeconds(0.05f);

    _isFrosted = true;

    yield return new WaitForSeconds(StatusConfig.FrostDuration);

    _isFrosted = false;
  }

  protected virtual void HandleIgnited(object sender, EventArgs e) {
    StartCoroutine(Ignite());
  }

  protected virtual IEnumerator Ignite() {
    for (int i = 0; i < StatusConfig.IgniteTicks; i++) {
      yield return new WaitForSeconds(StatusConfig.IgniteTimeBetweenTicks);

      Damage(StatusConfig.IgniteDamage);
    }
  }

  protected virtual void HandleElectrocuted(object sender, EventArgs e) {
    Damage(StatusConfig.ElectrocuteDamage);
  }

  public override void Damage(float amount) {
    base.Damage(amount + ((_isFrosted) ? amount * StatusConfig.FrostDamageMultiplier : 0));
  }

  public abstract void DamageWithStatuses(float amount, List<StatusCondition> statuses);

  public abstract void DamageWithStatusesAndType(float amount, List<StatusCondition> statuses, DamageType type);

  public virtual void DamageWithType(float amount, DamageType type) {
    float bonusDamage = (type == _weakTo) ? (amount * 0.5f) : 0;
    Damage(amount + bonusDamage);
  }
}
