using System;
using UnityEngine;

public class BagOfSugar : BaseItem {
  [SerializeField] private float _speedMultiplier;

  protected override void OnTriggerEnter2D(Collider2D other) {
    if (other.CompareTag("Player")) {
      GameObject pObject = other.gameObject;

      PlayerWeapons pWeapons = pObject?.GetComponent<PlayerWeapons>();
      pWeapons?.MultiplySpeed(_speedMultiplier);

      base.PickUp();
    }
  }
}
