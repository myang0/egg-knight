using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerFootprint : MonoBehaviour {
    public static EventHandler OnSandpitEnter;
    public static EventHandler OnSandpitExit;
    public GameObject parent;

    private void FixedUpdate() {
        transform.rotation = Quaternion.Euler (0.0f, 0.0f, parent.transform.rotation.z * -1.0f);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Sandpit")) {
            OnSandpitEnter?.Invoke(this, EventArgs.Empty);
        }
    }
    
    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Sandpit")) OnSandpitExit?.Invoke(this, EventArgs.Empty);
    }
}
