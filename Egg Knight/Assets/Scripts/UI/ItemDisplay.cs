using System;
using UnityEngine;

public class ItemDisplay : MonoBehaviour {
  [SerializeField] private GameObject _overlay;

  [SerializeField] private GameObject _itemGrid;
  [SerializeField] private GameObject _itemGridElementPrefab;

  private void Awake() {
    UIControls.OnTabHold += HandleTabHold;
    UIControls.OnTabRelease += HandleTabRelease;
    BaseItem.OnItemDisplay += HandleItemDisplay;
  }

  private void HandleTabHold(object sender, EventArgs e) {
    _overlay?.SetActive(true);
  }

  private void HandleTabRelease(object sender, EventArgs e) {
    _overlay?.SetActive(false);
  }

  private void HandleItemDisplay(object sender, ItemDisplayEventArgs e) {
    GameObject gridElementObject = Instantiate(_itemGridElementPrefab, Vector3.zero, Quaternion.identity);
    ItemGridElement gridElement = gridElementObject.GetComponent<ItemGridElement>();
    gridElement.Initialize(e.name, e.description, e.sprite);

    gridElementObject.transform.SetParent(_itemGrid.transform);
    gridElementObject.transform.localScale = Vector3.one;
  }

  private void OnDestroy() {
    UIControls.OnTabHold -= HandleTabHold;
    UIControls.OnTabRelease -= HandleTabRelease;
  }
}
