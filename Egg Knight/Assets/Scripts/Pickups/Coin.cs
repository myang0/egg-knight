using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private int _monetaryValue;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            GameObject playerObject = other.gameObject;
            PlayerWallet wallet = playerObject.GetComponent<PlayerWallet>();

            if (wallet != null) {
                wallet.AddToBalance(_monetaryValue);
            }

            Destroy(gameObject);
        }
    }
}
