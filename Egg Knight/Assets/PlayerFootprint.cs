using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerFootprint : MonoBehaviour {
    public static EventHandler OnSandpitEnter;
    public static EventHandler OnSandpitExit;
    
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Sandpit")) {
            OnSandpitEnter?.Invoke(this, EventArgs.Empty);
        }
    }
    
    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Sandpit")) OnSandpitExit?.Invoke(this, EventArgs.Empty);
    }
}
