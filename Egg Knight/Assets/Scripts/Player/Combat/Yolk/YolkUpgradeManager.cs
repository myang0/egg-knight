using System.Collections.Generic;
using UnityEngine;

public class YolkUpgradeManager : MonoBehaviour {
  private Dictionary<YolkUpgradeType, bool> _upgradeDict;

  private void Awake() {
    _upgradeDict = new Dictionary<YolkUpgradeType, bool>();

    YolkUpgrade.OnYolkUpgradePickup += HandleYolkUpgradePickup;
  }

  private void HandleYolkUpgradePickup(object sender, YolkUpgradeEventArgs e) {
    _upgradeDict.Add(e.type, true);
  }

  public bool HasUpgrade(YolkUpgradeType key) {
    return _upgradeDict.ContainsKey(key) && _upgradeDict[key] == true;
  }
}
