using System;
using System.Collections.Generic;
using UnityEngine;

public class PeaPodHealth : EnemyHealth {
  [SerializeField] private GameObject _peaSpawnerObject;

  public event EventHandler<EnemyStatusEventArgs> OnPeaPodStatusDamage;

  public override void DamageWithStatuses(float amount, List<StatusCondition> statuses) {
    OnPeaPodStatusDamage?.Invoke(this, new EnemyStatusEventArgs(statuses));

    Damage(amount);
  }

  public override void DamageWithStatusesAndType(float amount, List<StatusCondition> statuses, DamageType type) {
    OnPeaPodStatusDamage?.Invoke(this, new EnemyStatusEventArgs(statuses));

    DamageWithType(amount, type);
  }

  protected override void Die() {
    if (_peaSpawnerObject != null) {
      Instantiate(_peaSpawnerObject, transform.position, Quaternion.identity);
    }

    base.Die();
  }
}
