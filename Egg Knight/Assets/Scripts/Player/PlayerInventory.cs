using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour {
  private Dictionary<Item, int> _inventory;

  private void Awake() {
    BaseItem.OnInventoryAdd += AddItem;

    _inventory = new Dictionary<Item, int>();
  }

  private void AddItem(object sender, InventoryAddEventArgs e) {
    Item key = e.itemKey;

    if (ItemInInventory(key) == false) {
      _inventory.Add(key, 1);
    } else {
      _inventory[key]++;
    }
  }

  public void RemoveItem(Item key) {
    if (ItemInInventory(key)) {
      _inventory[key]--;
    }
  }

  public bool ItemInInventory(Item key) {
    return _inventory.ContainsKey(key);
  }

  public int GetItemQuantity(Item key) {
    return ItemInInventory(key) ? _inventory[key] : 0;
  }
}
