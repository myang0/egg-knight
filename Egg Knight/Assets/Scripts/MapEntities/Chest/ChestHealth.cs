using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestHealth : EnemyHealth
{
    public event EventHandler<EnemyStatusEventArgs> OnChestStatusDamage;

    public override void DamageWithStatuses(float amount, List<StatusCondition> statuses) {
        OnChestStatusDamage?.Invoke(this, new EnemyStatusEventArgs(statuses));

        Damage(amount);
    }

    public override void DamageWithStatusesAndType(float amount, List<StatusCondition> statuses, DamageType type) {
        OnChestStatusDamage?.Invoke(this, new EnemyStatusEventArgs(statuses));

        DamageWithType(amount, type);
    }
}
