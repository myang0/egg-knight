using System;
using System.Collections;
using System.Collections.Generic;
using Stage;
using UnityEngine;

public class Caravan : MonoBehaviour {
    public ShopItem shopItem1;
    public ShopItem shopItem2;
    public ShopItem shopItem3;

    public ItemManager itemManager;
    public LevelManager levelManager;
    private bool _hasDialoguePlayed;

    private void Start() {
        Vector3 currPos = transform.position;
        transform.position = new Vector3(currPos.x, currPos.y, ZcoordinateConsts.Interactable);
        itemManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<ItemManager>();
        shopItem1.item = itemManager.GetRandomHealingItem();
        shopItem2.item = itemManager.GetRandomItem();
        shopItem3.item = itemManager.GetRandomItem();
        levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
        _hasDialoguePlayed = false;
    }

    private void Update() {
        if (shopItem1 == shopItem2) {
            shopItem2.item = itemManager.GetRandomItem();
        }

        if (shopItem2 == shopItem3) {
            shopItem3.item = itemManager.GetRandomItem();
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player") && !_hasDialoguePlayed) {
            _hasDialoguePlayed = true;
            if (levelManager.isFirstShopVisited) {
                Fungus.Flowchart.BroadcastFungusMessage ("StageOneRepeatShop");
            }
            else {
                Fungus.Flowchart.BroadcastFungusMessage ("StageOneFirstShop");
                levelManager.isFirstShopVisited = true;
            }
        }
    }
}
