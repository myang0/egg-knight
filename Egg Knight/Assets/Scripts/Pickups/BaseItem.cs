using System;
using UnityEngine;

public class BaseItem : MonoBehaviour
{
    public event EventHandler OnPickup;
    
    protected virtual void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            PickUp();
        }
    }

    protected virtual void PickUp() {
        OnPickup?.Invoke(this, EventArgs.Empty);
        Destroy(gameObject);
    }
}
