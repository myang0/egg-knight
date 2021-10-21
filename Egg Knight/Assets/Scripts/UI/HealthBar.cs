using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {
  [SerializeField] private Slider _slider;

  private void Awake() {
    PlayerHealth.OnHealthChange += HandleHealthChange;
  }

  private void HandleHealthChange(object sender, PlayerHealthChangeEventArgs e) {
    _slider.value = e.newPercent;
  }
}
