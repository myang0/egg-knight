using System;
using UnityEngine;

public class EggShards : BaseItem {
  public static event EventHandler OnEggShardsPickup;

  protected override void OnTriggerEnter2D(Collider2D col) {
    if (col.CompareTag("Player")) {
      OnEggShardsPickup?.Invoke(this, EventArgs.Empty);

      PickUp();
    }
  }
}
