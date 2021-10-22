using System;
using UnityEngine;

public class PlayerWallet : MonoBehaviour {
  private int _balance = 0;

  public static event EventHandler<BalanceChangeEventArgs> OnBalanceChange;

  public int GetBalance() {
    return _balance;
  }

  public void AddToBalance(int amount) {
    _balance += amount;

    OnBalanceChange?.Invoke(this, new BalanceChangeEventArgs(_balance));
  }

  public bool MakeTransaction(int cost) {
    if ((_balance - cost) < 0) {
      return false;
    }

    _balance -= cost;

    OnBalanceChange?.Invoke(this, new BalanceChangeEventArgs(_balance));

    return true;
  }
}
