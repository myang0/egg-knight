using System;

public class InventoryAddEventArgs : EventArgs {
  public Item itemKey;

  public InventoryAddEventArgs(Item itemKey) {
    this.itemKey = itemKey;
  }
}
