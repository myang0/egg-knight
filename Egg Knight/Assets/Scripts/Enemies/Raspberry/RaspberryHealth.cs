using System;
using UnityEngine;

public class RaspberryHealth : EnemyHealth {
  public event EventHandler<EnemyStatusEventArgs> OnRaspberryStatusDamage;

  public override void DamageWithStatus(float amount, StatusCondition status) {
    OnRaspberryStatusDamage?.Invoke(this, new EnemyStatusEventArgs(status));

    Damage(amount);
  }
}
