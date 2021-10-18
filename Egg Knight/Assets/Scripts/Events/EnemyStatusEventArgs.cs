using System;
using System.Collections.Generic;

public class EnemyStatusEventArgs : EventArgs {
  public List<StatusCondition> statuses;

  public EnemyStatusEventArgs(List<StatusCondition> statuses) {
    this.statuses = statuses;
  }
}
