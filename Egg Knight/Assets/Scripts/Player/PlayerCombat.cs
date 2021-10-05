using System;
using System.Collections;
using UnityEngine;

public class PlayerCombat : MonoBehaviour {
  [SerializeField] private GameObject _yolkPrefab;

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

    Vector3 mousePosInWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    Vector2 vectorToMouse = VectorHelper.GetVectorToPoint(transform.position, mousePosInWorld);

    float angleToMouse = Vector2.SignedAngle(Vector2.up, vectorToMouse);

    GameObject yolkObject = Instantiate(_yolkPrefab, transform.position, Quaternion.identity);
    YolkProjectile yolk = yolkObject.GetComponent<YolkProjectile>();
    yolk.SetDirection(vectorToMouse, angleToMouse);

    yield return new WaitForSeconds(_yolkCooldown);

    _yolkOffCooldown = true;
  }
}
