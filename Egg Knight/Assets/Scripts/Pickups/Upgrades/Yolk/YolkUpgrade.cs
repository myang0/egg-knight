using System;
using UnityEngine;

public class YolkUpgrade : BaseItem {
  [SerializeField] private YolkUpgradeType _upgradeKey;

  public static event EventHandler<YolkUpgradeEventArgs> OnYolkUpgradePickup;

  protected override void OnTriggerEnter2D(Collider2D col) {
    if (col.CompareTag("Player")) {
      YolkPickup();
    }
  }

  protected void YolkPickup() {
    OnYolkUpgradePickup?.Invoke(this, new YolkUpgradeEventArgs(_upgradeKey));

    PickUp();
  }

  protected YolkManager GetYolkManager(Collider2D col) {
    GameObject playerObject = col.gameObject;
    return playerObject?.GetComponent<YolkManager>();
  }
}
