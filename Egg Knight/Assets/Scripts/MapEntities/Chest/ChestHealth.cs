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

    public override bool DamageWithStatusesAndType(float amount, List<StatusCondition> statuses, DamageType type) {
        bool isDamageDealt = DamageWithType(amount, type);
        if (isDamageDealt) OnChestStatusDamage?.Invoke(this, new EnemyStatusEventArgs(statuses));

        return isDamageDealt;
    }
}
