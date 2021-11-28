using System;
using UnityEngine;

public class RottenYolk : CursedItem {
  [SerializeField] private float _damageMultiplier;
  [SerializeField] private float _healMultiplier;

  protected override void OnTriggerEnter2D(Collider2D col) {
    if (col.CompareTag("Player")) {
      GameObject pObject = col.gameObject;

      YolkManager yolkManager = pObject?.GetComponent<YolkManager>();
      yolkManager?.MultiplyByDamageScaling(_damageMultiplier);

      PlayerHealth pHealth = pObject?.GetComponent<PlayerHealth>();
      pHealth?.ScaleHealMultiplier(_healMultiplier);

      CursedItemPickup();
    }
  }
}
