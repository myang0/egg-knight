using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockWeaponItem : MonoBehaviour {
    public static event EventHandler OnPickup;
    private float _origY;

    protected virtual void Awake() {
        _origY = transform.position.y;
    }

    protected virtual void Update() {
        transform.position = new Vector3(
            transform.position.x,
            _origY + (Mathf.Sin(Time.time) * 0.15f),
            transform.position.z
        );
    }
    
    protected virtual void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            OnPickup?.Invoke(this, EventArgs.Empty);
            Destroy(gameObject);
        }
    }
}
