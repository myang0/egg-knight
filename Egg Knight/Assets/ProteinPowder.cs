using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProteinPowder : BaseItem
{
    [SerializeField] private float _bonusDamageMultiplier;

    protected override void OnTriggerEnter2D(Collider2D col) {
        if (col.CompareTag("Player")) {
            GameObject pObject = col.gameObject;
            PlayerWeapons pWeapons = pObject?.GetComponent<PlayerWeapons>();
            pWeapons?.AddToDamageMultiplier(_bonusDamageMultiplier);

            base.PickUp();
        }
    }
}
