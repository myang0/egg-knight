using System;

public class RollEventArgs : EventArgs {
  public float duration;
  public Direction direction;

  public RollEventArgs(float duration, Direction direction) {
    this.duration = duration;
    this.direction = direction;
  }
}
