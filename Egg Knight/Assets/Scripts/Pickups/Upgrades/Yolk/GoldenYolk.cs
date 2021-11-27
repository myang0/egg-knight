using System;
using UnityEngine;

public class GoldenYolk : YolkUpgrade {
  [SerializeField] private float _costMultiplier;

  protected override void OnTriggerEnter2D(Collider2D col) {
    if (col.CompareTag("Player")) {
      YolkManager yolkManager = GetYolkManager(col);

      if (yolkManager != null) {
        yolkManager.MultiplyByCost(_costMultiplier);
      }

      YolkPickup();
    }
  }
}
