using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Caravan : MonoBehaviour {
    public ShopItem shopItem1;
    public ShopItem shopItem2;
    public ShopItem shopItem3;

    public ItemManager itemManager;

    private void Start() {
        shopItem1.item = itemManager.GetRandomItem();
        shopItem2.item = itemManager.GetRandomItem();
        shopItem3.item = itemManager.GetRandomItem();
    }

    private void Update() {
        if (shopItem1 == shopItem2) {
            shopItem2.item = itemManager.GetRandomItem();
        }

        if (shopItem2 == shopItem3) {
            shopItem3.item = itemManager.GetRandomItem();
        }
    }
}
