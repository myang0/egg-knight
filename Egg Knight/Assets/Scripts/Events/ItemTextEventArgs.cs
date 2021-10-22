using System;

public class ItemTextEventArgs : EventArgs {
  public string displayName;
  public string description;

  public ItemTextEventArgs(string displayName, string description) {
    this.displayName = displayName;
    this.description = description;
  }
}
