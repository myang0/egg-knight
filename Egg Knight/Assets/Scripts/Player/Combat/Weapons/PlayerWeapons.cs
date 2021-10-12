using System;
using UnityEngine;

public class PlayerWeapons : MonoBehaviour {
  readonly int NUM_WEAPON_TYPES = 3;

  [SerializeField] private GameObject[] _weapons;

  private int _currentWeaponIndex = 0;
  private GameObject _currentWeapon;

  private void Awake() {
    PlayerControls.OnQPress += SwitchPrevWeapon;
    PlayerControls.OnEPress += SwitchNextWeapon;

    PlayerControls.OnLeftClick += HandleAttack;
  }

  private void SwitchPrevWeapon(object sender, EventArgs e) {
    _currentWeaponIndex = (_currentWeaponIndex - 1 < 0) ? NUM_WEAPON_TYPES - 1 : _currentWeaponIndex - 1;
  }

  private void SwitchNextWeapon(object sender, EventArgs e) {
    _currentWeaponIndex = (_currentWeaponIndex + 1 >= NUM_WEAPON_TYPES) ? 0 : _currentWeaponIndex + 1;
  }

  private void HandleAttack(object sender, EventArgs e) {
    if (_currentWeapon == null) {
      float rotateAngle = anglesToMouse();

      _currentWeapon = Instantiate(_weapons[_currentWeaponIndex], transform.position, Quaternion.identity);
      _currentWeapon.transform.eulerAngles = new Vector3(0, 0, rotateAngle);
    }
  }

  private float anglesToMouse() {
    Vector3 mousePosInWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    Vector2 vectorToMouse = VectorHelper.GetVectorToPoint(transform.position, mousePosInWorld);

    return Vector2.SignedAngle(Vector2.up, vectorToMouse);
  }
}
