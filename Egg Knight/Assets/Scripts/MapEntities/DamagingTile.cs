using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagingTile : MonoBehaviour {
    public int damage;
    // Start is called before the first frame update
    void Start() {
        var pos = transform.position;
        transform.position = new Vector3(pos.x, pos.y, ZcoordinateConsts.Interactable);
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("PlayerFootprint")) 
            other.GetComponentInParent<PlayerHealth>().Damage(damage);
    }
    
    private void OnCollisionStay2D(Collision2D other) {
        if (other.collider.gameObject.layer == LayerMask.NameToLayer("PlayerFootprint")) 
            other.collider.GetComponentInParent<PlayerHealth>().Damage(damage);
    }
}
