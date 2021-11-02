using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {
  [SerializeField] private Slider _slider;

  private void Awake() {
    PlayerHealth.OnHealthIncrease += HandleHealthChange;
    PlayerHealth.OnHealthDecrease += HandleHealthChange;
  }

  private void HandleHealthChange(object sender, HealthChangeEventArgs e) {
    _slider.value = e.newPercent;
  }

  private void OnDestroy() {
    PlayerHealth.OnHealthIncrease -= HandleHealthChange;
    PlayerHealth.OnHealthDecrease -= HandleHealthChange;
  }
}
