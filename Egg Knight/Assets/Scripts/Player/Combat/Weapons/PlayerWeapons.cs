using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapons : MonoBehaviour {
  readonly int NUM_WEAPON_TYPES = 3;

  [SerializeField] private GameObject[] _weapons;
  [SerializeField] private GameObject[] _weaponDisplays; 

  private int _currentWeaponIndex = 0;

  private GameObject _currentWeapon;
  private GameObject _currentWeaponDisplay;

  private List<StatusCondition> _weaponModifiers;

  private Camera _mainCamera;

  private Transform _weaponHoldPoint;

  public static event EventHandler OnWeaponAnimationBegin;

  private void Awake() {
    PlayerControls.OnQPress += SwitchPrevWeapon;
    PlayerControls.OnEPress += SwitchNextWeapon;

    PlayerControls.OnLeftClick += HandleAttack;

    ElementalItem.OnElementalItemPickup += AddModifier;

    BasePlayerWeapon.OnWeaponAnimationEnd += HandleWeaponAnimEnd;

    _weaponModifiers = new List<StatusCondition>();

    _mainCamera = Camera.main;

    _weaponHoldPoint = GameObject.Find("PlayerWeaponDisplayPoint").transform;
    DisplayCurrentWeapon();
  }

  private void SwitchPrevWeapon(object sender, EventArgs e) {
    _currentWeaponIndex = (_currentWeaponIndex - 1 < 0) ? NUM_WEAPON_TYPES - 1 : _currentWeaponIndex - 1;
    DisplayCurrentWeapon();
  }

  private void SwitchNextWeapon(object sender, EventArgs e) {
    _currentWeaponIndex = (_currentWeaponIndex + 1 >= NUM_WEAPON_TYPES) ? 0 : _currentWeaponIndex + 1;
    DisplayCurrentWeapon();
  }

  private void HandleAttack(object sender, EventArgs e) {
    if (_currentWeapon == null) {
      OnWeaponAnimationBegin?.Invoke(this, EventArgs.Empty);
      
      _currentWeaponDisplay.SetActive(false);

      float rotateAngle = anglesToMouse();
      
      var wepPos = new Vector3(transform.position.x, transform.position.y, ZcoordinateConsts.WeaponAttack);
      _currentWeapon = Instantiate(_weapons[_currentWeaponIndex], wepPos, Quaternion.identity);
      _currentWeapon.transform.eulerAngles = new Vector3(0, 0, rotateAngle);

      if (_weaponModifiers.Any()) {
        BasePlayerWeapon weapon = _currentWeapon.GetComponent<BasePlayerWeapon>();
        weapon.SetModifiers(_weaponModifiers);
      }
    }
  }

  private void AddModifier(object sender, ElementalItemEventArgs e) {
    _weaponModifiers.Add(e.status);
  }

  private void HandleWeaponAnimEnd(object sender, EventArgs e) {
    _currentWeaponDisplay.SetActive(true);
  }

  private void DisplayCurrentWeapon() {
    if (_currentWeaponDisplay != null) {
      Destroy(_currentWeaponDisplay);
    }

    _currentWeaponDisplay = Instantiate(
      _weaponDisplays[_currentWeaponIndex],
      _weaponHoldPoint.position, 
      _weaponHoldPoint.localRotation
    );

    _currentWeaponDisplay.transform.SetParent(_weaponHoldPoint);
  }

  private float anglesToMouse() {
    Vector3 mousePosInWorld = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
    Vector2 vectorToMouse = VectorHelper.GetVectorToPoint(transform.position, mousePosInWorld);

    return Vector2.SignedAngle(Vector2.up, vectorToMouse);
  }

  private void OnDestroy() {
    PlayerControls.OnQPress -= SwitchPrevWeapon;
    PlayerControls.OnEPress -= SwitchNextWeapon;
    PlayerControls.OnLeftClick -= HandleAttack;
    ElementalItem.OnElementalItemPickup -= AddModifier;
    BasePlayerWeapon.OnWeaponAnimationEnd -= HandleWeaponAnimEnd;
  }
}
