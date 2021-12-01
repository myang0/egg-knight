using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : PickupBase
{
    [SerializeField] private GameObject _coinParticles;
    [SerializeField] private int _monetaryValue;

    [SerializeField] private AudioClip _clip;
    [SerializeField] private SingleTimeSound _singleTimeSound;

    public override void PickUp(GameObject playerObject) {
        PlayerWallet wallet = playerObject?.GetComponent<PlayerWallet>();

        wallet?.AddToBalance(_monetaryValue);

        if (_coinParticles != null) {
            Instantiate(_coinParticles, transform.position, Quaternion.identity);
        }

        Instantiate(_singleTimeSound, transform.position, Quaternion.identity)
            .GetComponent<SingleTimeSound>()
            .LoadClipAndPlay(_clip);

        Destroy(gameObject);
    }
}
