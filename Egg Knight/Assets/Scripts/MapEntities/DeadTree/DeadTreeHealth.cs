using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadTreeHealth : EnemyHealth
{
    public event EventHandler<EnemyStatusEventArgs> OnDeadTreeStatusDamage;

    public override void DamageWithStatuses(float amount, List<StatusCondition> statuses) {
        OnDeadTreeStatusDamage?.Invoke(this, new EnemyStatusEventArgs(statuses));

        Damage(amount);
    }

    public override void DamageWithStatusesAndType(float amount, List<StatusCondition> statuses, DamageType type) {
        OnDeadTreeStatusDamage?.Invoke(this, new EnemyStatusEventArgs(statuses));

        DamageWithType(amount, type);
    }
}
