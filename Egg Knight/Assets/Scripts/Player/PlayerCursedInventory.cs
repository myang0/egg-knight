using System.Collections.Generic;
using UnityEngine;

public class PlayerCursedInventory : MonoBehaviour {
  private Dictionary<CursedItemType, bool> _curseDict;

  private void Awake() {
    _curseDict = new Dictionary<CursedItemType, bool>();

    CursedItem.OnCursedItemPickup += HandleCursedItemPickup;
  }

  private void HandleCursedItemPickup(object sender, CursedItemEventArgs e) {
    _curseDict.Add(e.type, true);
  }

  public bool HasItem(CursedItemType key) {
    return _curseDict.ContainsKey(key) && _curseDict[key] == true;
  }
}
