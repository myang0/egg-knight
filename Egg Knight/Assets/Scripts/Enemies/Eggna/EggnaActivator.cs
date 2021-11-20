using System;
using UnityEngine;

public class EggnaActivator : MonoBehaviour {
  public static event EventHandler OnEggnaActivate;

  private void OnTriggerEnter2D(Collider2D other) {
    if (other.CompareTag("Player")) {
      OnEggnaActivate?.Invoke(this, EventArgs.Empty);
      Destroy(gameObject);
    }
  }
}
