using System;
using UnityEngine;

public class RocketPoweredYolk : YolkUpgrade {
  [SerializeField] private float _cooldownMultiplier;
  [SerializeField] private float _healthCostMultiplier;
  [SerializeField] private float _speedMultiplier;
  [SerializeField] private float _damageMultiplier;

  protected override void OnTriggerEnter2D(Collider2D col) {
    if (col.CompareTag("Player")) {
      YolkManager yolkManager = GetYolkManager(col);

      if (yolkManager != null) {
        yolkManager.MultiplyByCooldown(_cooldownMultiplier);
        yolkManager.MultiplyByPercentCost(_healthCostMultiplier);
        yolkManager.MultiplyBySpeedScaling(_speedMultiplier);
        yolkManager.MultiplyByDamageScaling(_damageMultiplier);
      }

      YolkPickup();
    }
  }
}
