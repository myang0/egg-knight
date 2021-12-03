using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomHealth : EnemyHealth
{
    private Animator _anim;

    [SerializeField] private GameObject _sporePrefab;

    public event EventHandler<EnemyStatusEventArgs> OnMushroomStatusDamage;

    protected override void Awake() {
        _anim = GetComponent<Animator>();

        base.Awake();

        StartCoroutine(SpawnSpores());
    }

    private IEnumerator SpawnSpores() {
        while (_currentHealth > 0) {
            if (_anim.GetBool("isAlert")) {
                Instantiate(_sporePrefab, transform.position, Quaternion.identity);
            }

            yield return new WaitForSeconds(1.5f);
        }
    }

    public override void DamageWithStatuses(float amount, List<StatusCondition> statuses) {
        OnMushroomStatusDamage?.Invoke(this, new EnemyStatusEventArgs(statuses));

        Damage(amount);
    }

    public override bool DamageWithStatusesAndType(float amount, List<StatusCondition> statuses, DamageType type) {
        bool isDamageDealt = DamageWithType(amount, type);
        if (isDamageDealt) OnMushroomStatusDamage?.Invoke(this, new EnemyStatusEventArgs(statuses));

        return isDamageDealt;
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
