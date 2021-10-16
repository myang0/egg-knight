using System;

public class ElementalItemEventArgs : EventArgs {
  public StatusCondition status;

  public ElementalItemEventArgs(StatusCondition status) {
    this.status = status;
  }
}
