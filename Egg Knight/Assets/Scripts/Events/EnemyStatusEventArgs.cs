using System;

public class EnemyStatusEventArgs : EventArgs {
  public StatusCondition status;

  public EnemyStatusEventArgs(StatusCondition status) {
    this.status = status;
  }
}
