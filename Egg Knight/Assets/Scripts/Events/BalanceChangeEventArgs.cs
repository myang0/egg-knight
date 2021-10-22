using System;

public class BalanceChangeEventArgs : EventArgs {
  public int newBalance;

  public BalanceChangeEventArgs(int newBalance) {
    this.newBalance = newBalance;
  }
}
