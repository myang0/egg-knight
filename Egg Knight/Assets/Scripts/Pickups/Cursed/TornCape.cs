using System;
using UnityEngine;

public class TornCape : CursedItem {
  [SerializeField] private int _maxHealthReduction;

  [SerializeField] private float _speedBonus;
  [SerializeField] private float _rollSpeedMultiplier;
  [SerializeField] private float _rollCooldownMultiplier;

  protected override void OnTriggerEnter2D(Collider2D col) {
    if (col.CompareTag("Player")) {
      GameObject pObject = col.gameObject;

      PlayerHealth pHealth = pObject?.GetComponent<PlayerHealth>();
      PlayerMovement pMovement = pObject?.GetComponent<PlayerMovement>();

      pHealth?.AddToMaxHealth(-_maxHealthReduction);

      pMovement?.IncreaseMoveSpeed(_speedBonus);
      pMovement?.MultiplyRollSpeed(_rollSpeedMultiplier);
      pMovement?.MultiplyRollCooldown(_rollCooldownMultiplier);

      CursedItemPickup();
    }
  }
}
