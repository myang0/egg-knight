using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private GameObject _coinParticles;
    [SerializeField] private int _monetaryValue;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            PickUp(other.gameObject);
        }
    }

    public void PickUp(GameObject playerObject) {
        PlayerWallet wallet = playerObject?.GetComponent<PlayerWallet>();

        wallet?.AddToBalance(_monetaryValue);

        if (_coinParticles != null) {
            Instantiate(_coinParticles, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}
