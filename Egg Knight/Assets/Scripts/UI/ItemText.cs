using System.Collections;
using UnityEngine;
using TMPro;

public class ItemText : MonoBehaviour {
  [SerializeField] private TextMeshProUGUI _nameText;
  [SerializeField] private TextMeshProUGUI _descriptionText;

  private Animator _anim;

  private void Awake() {
    _anim = GetComponent<Animator>();

    BaseItem.OnItemTextDisplay += HandleTextChange;
  }

  private void HandleTextChange(object sender, ItemTextEventArgs e) {
    _nameText.text = e.displayName;
    _descriptionText.text = e.description;

    _anim.Play("ItemScroll", -1, 0f);
  }

  private void OnDestroy() {
    BaseItem.OnItemTextDisplay -= HandleTextChange;
  }
}
