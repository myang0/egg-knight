using System;

public class YolkUpgradeEventArgs : EventArgs {
  public YolkUpgradeType type;

  public YolkUpgradeEventArgs(YolkUpgradeType type) {
    this.type = type;
  }
}
