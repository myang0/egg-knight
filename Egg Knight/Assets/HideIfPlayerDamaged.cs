using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideIfPlayerDamaged : MonoBehaviour {
    private PlayerHealth _pHealth;

    public SpriteRenderer sr;

    public Collider2D collider;
    // Start is called before the first frame update
    void Start() {
        _pHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_pHealth.CurrentHealth < 100) {
            if (sr != null) sr.enabled = false;
            if (collider != null) collider.enabled = false;
        }
        else {
            if (sr != null) sr.enabled = true;
            if (collider != null) collider.enabled = true;
        }
    }
}
