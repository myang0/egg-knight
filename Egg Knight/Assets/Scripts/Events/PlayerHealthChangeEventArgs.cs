using System;

public class PlayerHealthChangeEventArgs : EventArgs {
  public float currentHealth;
  public int maxHealth;

  public PlayerHealthChangeEventArgs(float currentHealth, int maxHealth) {
    this.currentHealth = currentHealth;
    this.maxHealth = maxHealth;
  }
}
