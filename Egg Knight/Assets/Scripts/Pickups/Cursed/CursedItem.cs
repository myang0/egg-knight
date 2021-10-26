using System;
using UnityEngine;

public class CursedItem : BaseItem {
  [SerializeField] private CursedItemType _curseKey;

  public static event EventHandler<CursedItemEventArgs> OnCursedItemPickup;

  protected override void OnTriggerEnter2D(Collider2D col) {
    if (col.CompareTag("Player")) {
      CursedItemPickup();
    }
  }

  protected void CursedItemPickup() {
    OnCursedItemPickup?.Invoke(this, new CursedItemEventArgs(_curseKey));

    PickUp();
  }
}
