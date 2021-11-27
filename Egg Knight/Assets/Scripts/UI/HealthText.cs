using System;
using UnityEngine;
using TMPro;

public class HealthText : MonoBehaviour {
  [SerializeField] private TextMeshProUGUI _text;
  
  private void Awake() {
    PlayerHealth.OnHealthChange += HandleHealthChange;
  }

  private void HandleHealthChange(object sender, PlayerHealthChangeEventArgs e) {
    if (_text != null) {
      string currentHealthString = (e.currentHealth < 1.0f) ? e.currentHealth.ToString("0.##") : e.currentHealth.ToString("#.##");
      _text.text = $"{currentHealthString}/{e.maxHealth}";
    }
  }

  private void OnDestroy() {
    PlayerHealth.OnHealthChange += HandleHealthChange;
  }
}
