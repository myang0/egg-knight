using System;
using System.Collections;
using UnityEngine;

public class PlayerCombat : MonoBehaviour {
  [SerializeField] private float _yolkCooldown = 2.0f;
  private bool _yolkOffCooldown = true;

  private void Awake() {
    PlayerControls.OnSpaceBarPressed += HandleSpaceBarPress;
  }

  private void HandleSpaceBarPress(object sender, EventArgs e) {
    if (_yolkOffCooldown) {
      // Broadcast event that yolk has been shot

      StartCoroutine(ShootYolk());
    }
  }

  IEnumerator ShootYolk() {
    _yolkOffCooldown = false;

    yield return new WaitForSeconds(_yolkCooldown);

    _yolkOffCooldown = true;
  }
}
