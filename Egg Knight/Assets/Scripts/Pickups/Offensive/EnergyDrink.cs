using System;
using UnityEngine;

public class EnergyDrink : BaseItem {
  [SerializeField] private float _attackSpeedModifier;
  [SerializeField] private float _attackModifier;

  protected override void OnTriggerEnter2D(Collider2D other) {
    if (other.CompareTag("Player")) {
      GameObject pObject = other.gameObject;

      PlayerWeapons pWeapons = pObject?.GetComponent<PlayerWeapons>();
      pWeapons?.MultiplySpeed(_attackSpeedModifier);
      pWeapons?.ScaleDamageMultiplier(_attackModifier);

      base.PickUp();
    }
  }
}
