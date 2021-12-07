using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatIncreaseItem : BaseItem
{
    [SerializeField] private float _bonusHealth;
    [SerializeField] private float _bonusDamageFlat;
    [SerializeField] private float _bonusDamageMultiplier;
    [SerializeField] private float _attackSpeedBonus;
    [SerializeField] private float _speedMultiplier;

    protected override void OnTriggerEnter2D(Collider2D col) {
        if (col.CompareTag("Player")) {
            GameObject pObject = col.gameObject;

            PlayerMovement pMovement = pObject?.GetComponent<PlayerMovement>();
            PlayerHealth pHealth = pObject?.GetComponent<PlayerHealth>();
            PlayerWeapons pWeapons = pObject?.GetComponent<PlayerWeapons>();

            if (_speedMultiplier != 0) pMovement?.MultiplyMoveSpeed(_speedMultiplier);
            if (_bonusHealth != 0) pHealth?.AddToMaxHealth(_bonusHealth);
            if (_attackSpeedBonus != 0) pWeapons?.AddToSpeedMultiplier(_attackSpeedBonus);
            if (_bonusDamageFlat != 0) pWeapons?.AddToDamageMultiplier(_bonusDamageFlat);
            if (_bonusDamageMultiplier != 0) pWeapons?.ScaleDamageMultiplier(_bonusDamageMultiplier);

            base.PickUp();
        }
    }
}
