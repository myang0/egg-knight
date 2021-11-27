using UnityEngine;
using UnityEngine.UI;

public class YolkBar : MonoBehaviour {
  [SerializeField] private Slider _slider;

  private void Awake() {
    YolkManager.OnYolkChange += HandleYolkChange;
  }

  private void HandleYolkChange(object sender, YolkChangeEventArgs e) {
    _slider.value = e.newPercent;
  }

  private void OnDestroy() {
    YolkManager.OnYolkChange -= HandleYolkChange;
  }
}
