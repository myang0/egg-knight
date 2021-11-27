using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerFootprint : MonoBehaviour {
    public static EventHandler OnSandpitEnter;
    public static EventHandler OnSandpitExit;
    public GameObject parent;
    
    private void Start() {
        PlayerMovement.OnRollBegin += DisableEnemyCollision;
        PlayerMovement.OnRollEnd += EnableEnemyCollision;

        PlayerHealth.OnNinjaIFramesEnabled += HandleNinjaIFramesEnabled;
        PlayerHealth.OnNinjaIFramesDisabled += HandleNinjaIFramesDisabled;
    }

    private void DisableEnemyCollision(object sender, RollEventArgs e) {
        Physics2D.IgnoreLayerCollision(this.gameObject.layer, LayerMask.NameToLayer("Enemy"), true);
    }

    private void EnableEnemyCollision(object sender, EventArgs e) {
        Physics2D.IgnoreLayerCollision(this.gameObject.layer, LayerMask.NameToLayer("Enemy"), false);
    }

    private void HandleNinjaIFramesEnabled(object sender, EventArgs e) {
        Physics2D.IgnoreLayerCollision(this.gameObject.layer, LayerMask.NameToLayer("Enemy"), true);
    }

    private void HandleNinjaIFramesDisabled(object sender, EventArgs e) {
        Physics2D.IgnoreLayerCollision(this.gameObject.layer, LayerMask.NameToLayer("Enemy"), false);
    }

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

    private void OnDestroy() {
        PlayerMovement.OnRollBegin -= DisableEnemyCollision;
        PlayerMovement.OnRollEnd -= EnableEnemyCollision;

        PlayerHealth.OnNinjaIFramesEnabled -= HandleNinjaIFramesEnabled;
        PlayerHealth.OnNinjaIFramesDisabled -= HandleNinjaIFramesDisabled;
    }
}
