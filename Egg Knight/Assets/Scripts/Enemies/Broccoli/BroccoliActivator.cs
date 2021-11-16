using System;
using UnityEngine;

public class BroccoliActivator : MonoBehaviour {
  public static event EventHandler OnBroccoliActivate;

  private void OnTriggerEnter2D(Collider2D other) {
    if (other.CompareTag("Player")) {
      OnBroccoliActivate?.Invoke(this, EventArgs.Empty);
      Destroy(gameObject);
    }
  }  
}
