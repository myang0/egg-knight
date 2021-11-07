using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemGridElement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
  [SerializeField] private GameObject _imageObject;

  private Image _image;

  private string _itemName;
  private string _itemDescription;

  private void Awake() {
    _image = _imageObject.GetComponent<Image>();
  }

  public void Initialize(string name, string description, Sprite sprite) {
    _image.sprite = sprite;
  }

  public void OnPointerEnter(PointerEventData eventData) {
    Debug.Log("hovering");
  }

  public void OnPointerExit(PointerEventData eventData) {
    Debug.Log("no longer hovering");
  }
}
