using System;
using UnityEngine;
using TMPro;

public class TooltipScreenSpaceUI : MonoBehaviour {
  [SerializeField] private RectTransform _canvasRectTransform;

  [SerializeField] private RectTransform _backgroundRectTransform;
  [SerializeField] private TextMeshProUGUI _text;

  [SerializeField] private GameObject _tooltip;

  private RectTransform _rectTransform;
  
  private void Awake() {
    _rectTransform = GetComponent<RectTransform>();

    ItemGridElement.OnTooltipEnable += HandleTooltipEnable;
    UIControls.OnTabRelease += HandleTooltipDisable;
  }

  private void HandleTooltipEnable(object sender, TooltipEnableEventArgs e) {
    _tooltip.SetActive(true);
    SetText($"{e.title}\n\n{e.subtitle}");
  }

  private void HandleTooltipDisable(object sender, EventArgs e) {
    SetText("");
    _tooltip.SetActive(false);
  }

  private void SetText(string newText) {
    _text.SetText(newText);
    _text.ForceMeshUpdate();

    Vector2 textSize = _text.GetRenderedValues(false);
    Vector2 paddingSize = new Vector2(20, 20);

    _backgroundRectTransform.sizeDelta = textSize + paddingSize;
  }

  private void Update() {
    Vector2 anchoredPosition = Input.mousePosition / _canvasRectTransform.localScale.x;

    if (anchoredPosition.x + _backgroundRectTransform.rect.width > _canvasRectTransform.rect.width) {
      anchoredPosition.x = _canvasRectTransform.rect.width - _backgroundRectTransform.rect.width;
    }

    if (anchoredPosition.y + _backgroundRectTransform.rect.height > _canvasRectTransform.rect.height) {
      anchoredPosition.y = _canvasRectTransform.rect.height - _backgroundRectTransform.rect.height;
    }

    _rectTransform.anchoredPosition = anchoredPosition;
  }

  private void OnDestroy() {
    ItemGridElement.OnTooltipEnable -= HandleTooltipEnable;
    UIControls.OnTabRelease -= HandleTooltipDisable;
  }
}
