using System;

public class PlayerHealthChangeEventArgs : EventArgs {
  public float newPercent;

  public PlayerHealthChangeEventArgs(float percent) {
    this.newPercent = percent;
  }
}
