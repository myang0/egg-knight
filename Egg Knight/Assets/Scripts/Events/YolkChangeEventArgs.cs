using System;

public class YolkChangeEventArgs : EventArgs {
  public float newPercent;

  public YolkChangeEventArgs(float percent) {
    this.newPercent = percent;
  }
}
