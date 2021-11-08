using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemGridElement : MonoBehaviour, IPointerEnterHandler {
  [SerializeField] private GameObject _imageObject;

  public static event EventHandler<TooltipEnableEventArgs> OnTooltipEnable;

  private Image _image;

  private string _itemName;
  private string _itemDescription;

  private void Awake() {
    _image = _imageObject.GetComponent<Image>();
  }

  public void Initialize(string name, string description, Sprite sprite) {
    _itemName = name;
    _itemDescription = description;

    _image.sprite = sprite;
  }

  public void OnPointerEnter(PointerEventData eventData) {
    OnTooltipEnable?.Invoke(this, new TooltipEnableEventArgs(_itemName, _itemDescription));
  }
}
