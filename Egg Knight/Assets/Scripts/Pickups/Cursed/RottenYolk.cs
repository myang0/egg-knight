using System;
using UnityEngine;

public class RottenYolk : CursedItem {
  [SerializeField] private float _damageMultiplier;

  protected override void OnTriggerEnter2D(Collider2D col) {
    if (col.CompareTag("Player")) {
      GameObject pObject = col.gameObject;

      YolkManager yolkManager = pObject?.GetComponent<YolkManager>();
      yolkManager?.MultiplyByDamageScaling(_damageMultiplier);

      CursedItemPickup();
    }
  }
}
