using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomHealth : EnemyHealth
{
    [SerializeField] private GameObject _sporePrefab;

    public event EventHandler<EnemyStatusEventArgs> OnMushroomStatusDamage;

    public override void DamageWithStatuses(float amount, List<StatusCondition> statuses) {
        OnMushroomStatusDamage?.Invoke(this, new EnemyStatusEventArgs(statuses));

        Damage(amount);
    }

    public override void DamageWithStatusesAndType(float amount, List<StatusCondition> statuses, DamageType type) {
        OnMushroomStatusDamage?.Invoke(this, new EnemyStatusEventArgs(statuses));

        DamageWithType(amount, type);
    }

    protected override void Die() {
        if (_sporePrefab != null) {
            for (int i = 0; i < 5; i++) {
                Instantiate(_sporePrefab, transform.position, Quaternion.identity);
            }
        }

        base.Die();
    }
}
