using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingDummyHealth : EnemyHealth
{
    public event EventHandler<EnemyStatusEventArgs> OnTrainingDummyStatusDamage;

    public override void DamageWithStatuses(float amount, List<StatusCondition> statuses) {
        OnTrainingDummyStatusDamage?.Invoke(this, new EnemyStatusEventArgs(statuses));

        Damage(amount);
    }

    public override void DamageWithStatusesAndType(float amount, List<StatusCondition> statuses, DamageType type) {
        OnTrainingDummyStatusDamage?.Invoke(this, new EnemyStatusEventArgs(statuses));

        DamageWithType(amount, type);
    }
}
