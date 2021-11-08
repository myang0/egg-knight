using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagingTerrain : MonoBehaviour {
    public int damage;
    // Start is called before the first frame update
    void Start() {
        var pos = transform.position;
        transform.position = new Vector3(pos.x, pos.y, ZcoordinateConsts.Interactable);
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (other.CompareTag("Player")) other.GetComponent<PlayerHealth>().Damage(damage);
    }
}
