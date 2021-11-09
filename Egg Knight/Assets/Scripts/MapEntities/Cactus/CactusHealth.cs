using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CactusHealth : EnemyHealth
{
    public event EventHandler<EnemyStatusEventArgs> OnCactusStatusDamage;

    public override void DamageWithStatuses(float amount, List<StatusCondition> statuses) {
        OnCactusStatusDamage?.Invoke(this, new EnemyStatusEventArgs(statuses));

        Damage(amount);
    }

    public override void DamageWithStatusesAndType(float amount, List<StatusCondition> statuses, DamageType type) {
        OnCactusStatusDamage?.Invoke(this, new EnemyStatusEventArgs(statuses));

        DamageWithType(amount, type);
    }
}
