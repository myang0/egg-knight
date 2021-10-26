using System;

public class CursedItemEventArgs : EventArgs {
  public CursedItemType type;

  public CursedItemEventArgs(CursedItemType type) {
    this.type = type;
  }
}
