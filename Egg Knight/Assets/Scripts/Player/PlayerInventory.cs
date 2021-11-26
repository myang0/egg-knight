using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour {
  private Dictionary<Item, int> _inventory = new Dictionary<Item, int>();

  private void Awake() {
    BaseItem.OnInventoryAdd += AddItem;

    _inventory = new Dictionary<Item, int>();
  }

  private void AddItem(object sender, InventoryAddEventArgs e) {
    Item key = e.itemKey;

    if (HasItem(key) == false) {
      _inventory.Add(key, 1);
    } else {
      _inventory[key]++;
    }
  }

  public void RemoveItem(Item key) {
    if (HasItem(key)) {
      _inventory[key]--;
    }
  }

  public bool HasItem(Item key) {
    return _inventory.ContainsKey(key);
  }

  public int GetItemQuantity(Item key) {
    return HasItem(key) ? _inventory[key] : 0;
  }

  private void OnDestroy() {
    BaseItem.OnInventoryAdd -= AddItem;
  }
}
