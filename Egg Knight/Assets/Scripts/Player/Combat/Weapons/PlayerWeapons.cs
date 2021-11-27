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
  public static event EventHandler OnSwitchKnife;
  public static event EventHandler OnSwitchFork;
  public static event EventHandler OnSwitchSpoon;

  private bool _isForkUnlocked = false;
  private bool _isSpoonUnlocked = false;

  private void Awake() {
    // PlayerControls.OnQPress += SwitchPrevWeapon;
    // PlayerControls.OnEPress += SwitchNextWeapon;
    UnlockWeaponItem.OnPickup += UnlockWeapon;
    PlayerControls.OnUnlockAllWeapons += UnlockAllWeapons;
    PlayerControls.On1Press += SwitchKnife;
    PlayerControls.On2Press += SwitchFork;
    PlayerControls.On3Press += SwitchSpoon;
    PlayerControls.OnScrollUp += SwitchNextWeapon;
    PlayerControls.OnScrollDown += SwitchPrevWeapon;
    
    PlayerControls.OnLeftClick += HandleAttack;

    ElementalItem.OnElementalItemPickup += AddModifier;

    BasePlayerWeapon.OnWeaponAnimationEnd += HandleWeaponAnimEnd;

    _weaponModifiers = new List<StatusCondition>();

    _mainCamera = Camera.main;

    _weaponHoldPoint = GameObject.Find("PlayerWeaponDisplayPoint").transform;
    DisplayCurrentWeapon();
  }

  private void UnlockWeapon(object sender, EventArgs e) {
    if (!_isForkUnlocked) _isForkUnlocked = true;
    else if (!_isSpoonUnlocked) _isSpoonUnlocked = true;
  }

  private void UnlockAllWeapons(object sender, EventArgs e) {
    _isForkUnlocked = true;
    _isSpoonUnlocked = true;
  }

  private void SwitchPrevWeapon(object sender, EventArgs e) {
    _currentWeaponIndex = (_currentWeaponIndex - 1 < 0) ? NUM_WEAPON_TYPES - 1 : _currentWeaponIndex - 1;
    if (!_isForkUnlocked && !_isSpoonUnlocked) _currentWeaponIndex = 0;
    if (_isForkUnlocked && !_isSpoonUnlocked && _currentWeaponIndex == 2) _currentWeaponIndex = 1;
    DisplayCurrentWeapon();
    
    if (_currentWeaponIndex == 0) OnSwitchKnife?.Invoke(this, EventArgs.Empty);
    else if (_currentWeaponIndex == 1) OnSwitchFork?.Invoke(this, EventArgs.Empty);
    else if (_currentWeaponIndex == 2) OnSwitchSpoon?.Invoke(this, EventArgs.Empty);
  }

  private void SwitchNextWeapon(object sender, EventArgs e) {
    _currentWeaponIndex = (_currentWeaponIndex + 1 >= NUM_WEAPON_TYPES) ? 0 : _currentWeaponIndex + 1;
    if (!_isForkUnlocked && !_isSpoonUnlocked) _currentWeaponIndex = 0;
    if (_isForkUnlocked && !_isSpoonUnlocked && _currentWeaponIndex == 2) _currentWeaponIndex = 0;
    DisplayCurrentWeapon();
    
    if (_currentWeaponIndex == 0) OnSwitchKnife?.Invoke(this, EventArgs.Empty);
    else if (_currentWeaponIndex == 1) OnSwitchFork?.Invoke(this, EventArgs.Empty);
    else if (_currentWeaponIndex == 2) OnSwitchSpoon?.Invoke(this, EventArgs.Empty);
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
    PlayerControls.On1Press -= SwitchKnife;
    PlayerControls.On2Press -= SwitchFork;
    PlayerControls.On3Press -= SwitchSpoon;
    PlayerControls.OnScrollUp -= SwitchNextWeapon;
    PlayerControls.OnScrollDown -= SwitchPrevWeapon;
    // PlayerControls.OnQPress -= SwitchPrevWeapon;
    // PlayerControls.OnEPress -= SwitchNextWeapon;
    PlayerControls.OnLeftClick -= HandleAttack;
    ElementalItem.OnElementalItemPickup -= AddModifier;
    BasePlayerWeapon.OnWeaponAnimationEnd -= HandleWeaponAnimEnd;
  }
  
  private void SwitchKnife(object sender, EventArgs e) {
    if (_currentWeaponIndex != 0) {
      _currentWeaponIndex = 0;
      DisplayCurrentWeapon();
      OnSwitchKnife?.Invoke(this, EventArgs.Empty);
    }
  }
  
  private void SwitchFork(object sender, EventArgs e) {
    if (!_isForkUnlocked) return;
    if (_currentWeaponIndex != 1) {
      _currentWeaponIndex = 1;
      DisplayCurrentWeapon();
      OnSwitchFork?.Invoke(this, EventArgs.Empty);
    }
  }
  
  private void SwitchSpoon(object sender, EventArgs e) {
    if (!_isSpoonUnlocked) return;
    if (_currentWeaponIndex != 2) {
      _currentWeaponIndex = 2;
      DisplayCurrentWeapon();
      OnSwitchSpoon?.Invoke(this, EventArgs.Empty);
    }
  }
}
