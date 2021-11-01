using System;

public class BossSpawnEventArgs : EventArgs {
  public string name;

  public BossSpawnEventArgs(string name) {
    this.name = name;
  }
}
