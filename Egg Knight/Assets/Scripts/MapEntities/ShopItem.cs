using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItem : MonoBehaviour {
    public BuyZone buyZone;
    public BaseItem item;
    public int price;
    private GameObject _playerObj;
    private PlayerWallet _playerWallet;
    private SpriteRenderer _sr;
    private WaveCounterText _waveCounterText;

    private void Awake() {
        _sr = gameObject.GetComponent<SpriteRenderer>();
        _playerObj = GameObject.FindGameObjectWithTag("Player");
        _playerWallet = _playerObj.GetComponent<PlayerWallet>();
        _waveCounterText = FindObjectOfType<WaveCounterText>();
    }

    private void Update() {
        if (buyZone.isPlayerInBuyZone) ShowBuyItemMessage();
        if (buyZone.isPlayerInBuyZone && Input.GetKey(KeyCode.F)) BuyItem();
        if (item && _sr.sprite != item.GetComponent<SpriteRenderer>().sprite) _sr.sprite = item.GetComponent<SpriteRenderer>().sprite;
    }

    private void BuyItem() {
        if (_playerWallet.GetBalance() >= price) {
            CompleteTransaction();
        }
    }
    private void ShowBuyItemMessage() {
        if (_playerWallet.GetBalance() >= price) {
            _waveCounterText.SetText("Press F to buy " + item.DisplayName + " for " + price + " coins", 0);
        }
        else {
            _waveCounterText.SetText("Not enough coins to purchase " + item.DisplayName, 0);
        }
    }

    private void CompleteTransaction() {
        _playerWallet.MakeTransaction(price);
        Vector3 playerPos = _playerObj.transform.position;
        Instantiate(item, playerPos, Quaternion.identity);
        ItemManager itemManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<ItemManager>();
        itemManager._items.Remove(item);
        Destroy(gameObject);
    }
}
