using System;

public class HealthChangeEventArgs : EventArgs {
  public float newPercent;

  public HealthChangeEventArgs(float percent) {
    this.newPercent = percent;
  }
}
