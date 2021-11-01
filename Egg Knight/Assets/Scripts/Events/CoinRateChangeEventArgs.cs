using System;

public class CoinRateChangeEventArgs : EventArgs {
  public float rate;

  public CoinRateChangeEventArgs(float rate) {
    this.rate = rate;
  }
}
