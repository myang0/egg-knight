using System;
using System.Collections;
using System.Collections.Generic;
using Stage;
using UnityEngine;

public class Caravan : MonoBehaviour {
    public ShopItem shopItem1;
    public ShopItem shopItem2;
    public ShopItem shopItem3;
    public ShopItem shopItem4;
    public ShopItem shopItem5;

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
        if (shopItem4) shopItem4.item = itemManager.GetRandomItem();
        if (shopItem5) shopItem5.item = itemManager.GetRandomItem();
        levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
        _hasDialoguePlayed = false;

        Vector3 pos = transform.position;
        transform.position = new Vector3(pos.x, pos.y, ZcoordinateConsts.Interactable);
    }

    private void Update() {
        if (shopItem1.item == shopItem2.item || shopItem1.item == shopItem3.item) {
            shopItem1.item = itemManager.GetRandomItem();
        }

        if (shopItem2.item == shopItem3.item) {
            shopItem2.item = itemManager.GetRandomItem();
        }

        if (shopItem4 && shopItem5) {
            if (shopItem4.item == shopItem5.item) {
                shopItem4.item = itemManager.GetRandomItem();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player") && !_hasDialoguePlayed) {
            _hasDialoguePlayed = true;
            if (levelManager.level == 1) {
                if (levelManager.isFirstShopVisited) {
                    Fungus.Flowchart.BroadcastFungusMessage ("StageOneRepeatShop");
                }
                else {
                    Fungus.Flowchart.BroadcastFungusMessage ("StageOneFirstShop");
                    levelManager.isFirstShopVisited = true;
                }
            } else if (levelManager.level == 2) {
                if (levelManager.isFirstShopVisited) {
                    Fungus.Flowchart.BroadcastFungusMessage ("StageTwoRepeatShop");
                }
                else {
                    Fungus.Flowchart.BroadcastFungusMessage ("StageTwoFirstShop");
                    levelManager.isFirstShopVisited = true;
                }
            } else if (levelManager.level == 3) {
                if (levelManager.isFirstShopVisited) {
                    Fungus.Flowchart.BroadcastFungusMessage ("StageThreeRepeatShop");
                }
                else {
                    Fungus.Flowchart.BroadcastFungusMessage ("StageThreeFirstShop");
                    levelManager.isFirstShopVisited = true;
                }
            }
        }
    }
}
