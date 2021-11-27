using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedWallHealth : EnemyHealth
{
    public event EventHandler<EnemyStatusEventArgs> OnLockedWallStatusDamage;

    public override void DamageWithStatuses(float amount, List<StatusCondition> statuses) {
        OnLockedWallStatusDamage?.Invoke(this, new EnemyStatusEventArgs(statuses));

        Damage(amount);
    }

    public override bool DamageWithStatusesAndType(float amount, List<StatusCondition> statuses, DamageType type) {
        OnLockedWallStatusDamage?.Invoke(this, new EnemyStatusEventArgs(statuses));

        return DamageWithType(amount, type);
    }
}
