using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public event EventHandler OnPickup;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            //todo
            Debug.Log("Todo: PowerUp Pickup");
            OnPickup?.Invoke(this, EventArgs.Empty);
            Destroy(gameObject);
        }
    }
}
