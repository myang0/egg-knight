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

    public override bool DamageWithStatusesAndType(float amount, List<StatusCondition> statuses, DamageType type) {
        bool isDamageDealt = DamageWithType(amount, type);
        if (isDamageDealt) OnDeadTreeStatusDamage?.Invoke(this, new EnemyStatusEventArgs(statuses));

        return isDamageDealt;
    }
}
