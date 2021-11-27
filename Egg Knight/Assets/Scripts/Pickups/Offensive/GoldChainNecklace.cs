using System;
using UnityEngine;

public class GoldChainNecklace : BaseItem {
  [SerializeField] private float _additionalDropRate;

  public static event EventHandler<CoinRateChangeEventArgs> OnPickup;

  protected override void OnTriggerEnter2D(Collider2D other) {
    if (other.CompareTag("Player")) {
      OnPickup?.Invoke(this, new CoinRateChangeEventArgs(_additionalDropRate));

      base.PickUp();
    }
  }
}
