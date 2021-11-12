using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PickupBase : MonoBehaviour {
    public bool allowWeaponPickup;
    
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            PickUp(other.gameObject);
        }
    }

    public abstract void PickUp(GameObject playerObject);
}
