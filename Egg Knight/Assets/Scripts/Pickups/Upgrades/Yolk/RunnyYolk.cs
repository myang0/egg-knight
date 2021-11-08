using System;
using UnityEngine;

public class RunnyYolk : YolkUpgrade {
  [SerializeField] private float _cooldownMultiplier;
  [SerializeField] private float _healthCostMultiplier;
  [SerializeField] private float _damageMultiplier;

  protected override void OnTriggerEnter2D(Collider2D col) {
    if (col.CompareTag("Player")) {
      YolkManager yolkManager = GetYolkManager(col);

      if (yolkManager != null) {
        yolkManager.MultiplyByCooldown(_cooldownMultiplier);
        yolkManager.MultiplyByPercentCost(_healthCostMultiplier);
        yolkManager.MultiplyByDamageScaling(_damageMultiplier);
      }

      YolkPickup();
    }
  }
}
