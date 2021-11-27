using System;
using UnityEngine;

public class RunnyYolk : YolkUpgrade {
  [SerializeField] private float _cooldownMultiplier;
  [SerializeField] private float _costMultiplier;
  [SerializeField] private float _damageMultiplier;

  protected override void OnTriggerEnter2D(Collider2D col) {
    if (col.CompareTag("Player")) {
      YolkManager yolkManager = GetYolkManager(col);

      if (yolkManager != null) {
        yolkManager.MultiplyByCooldown(_cooldownMultiplier);
        yolkManager.MultiplyByCost(_costMultiplier);
        yolkManager.MultiplyByDamageScaling(_damageMultiplier);
      }

      YolkPickup();
    }
  }
}
