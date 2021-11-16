using System;
using UnityEngine;

public class SausageActivator : MonoBehaviour {
  public static event EventHandler OnSausageActivate;

  private void OnTriggerEnter2D(Collider2D other) {
    if (other.CompareTag("Player")) {
      OnSausageActivate?.Invoke(this, EventArgs.Empty);
      Destroy(gameObject);
    }
  }
}
