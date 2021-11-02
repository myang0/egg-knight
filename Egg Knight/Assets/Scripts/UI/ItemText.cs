using System.Collections;
using UnityEngine;
using TMPro;

public class ItemText : MonoBehaviour {
  [SerializeField] private TextMeshProUGUI _nameText;
  [SerializeField] private TextMeshProUGUI _descriptionText;

  [SerializeField] private float _displayTime;

  private void Awake() {
    BaseItem.OnItemTextDisplay += HandleTextChange;
  }

  private void HandleTextChange(object sender, ItemTextEventArgs e) {
    _nameText.text = e.displayName;
    _descriptionText.text = e.description;

    _nameText.alpha = 0;
    _descriptionText.alpha = 0;

    StopAllCoroutines();
    StartCoroutine(FadeIn());
  }

  private IEnumerator FadeIn() {
    float alpha = 0;

    while (alpha < 1) {
      _nameText.alpha = alpha + 0.02f;
      _descriptionText.alpha = alpha + 0.02f;
      
      alpha += 0.02f;

      yield return new WaitForSeconds(0.01f);
    }

    yield return new WaitForSeconds(_displayTime);

    StartCoroutine(FadeOut());
  }

  private IEnumerator FadeOut() {
    float alpha = 1;

    while (alpha > 0) {
      _nameText.alpha = alpha - 0.02f;
      _descriptionText.alpha = alpha - 0.02f;
      
      alpha -= 0.02f;

      yield return new WaitForSeconds(0.01f);
    }
  }

  private void OnDestroy() {
    BaseItem.OnItemTextDisplay -= HandleTextChange;
  }
}
