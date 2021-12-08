using System;
using System.Linq;
using System.Collections;
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

  private PlayerInventory _inventory;
  private Transform _weaponHoldPoint;

  public static event EventHandler OnWeaponAnimationBegin;
  public static event EventHandler OnSwitchKnife;
  public static event EventHandler OnSwitchFork;
  public static event EventHandler OnSwitchSpoon;
  public static event EventHandler OnAttackSpeedChange;
  public static event EventHandler OnDamageMultiplierChange;

  private bool _isForkUnlocked = false;
  private bool _isSpoonUnlocked = false;

  private float _speed = 1.0f;
  private float _tempSpeed = 1.0f;
  private float _damageMultiplier = 1.0f;
  
  private float _berserkerSpeed = 0f;
  private bool _isBerserkerDecaying = false;

  private bool _knifeBeamOffCooldown = true;

  private void Awake() {
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
    BasePlayerWeapon.OnValidAttack += AddBerserkerSpeed;

    _weaponModifiers = new List<StatusCondition>();

    _mainCamera = Camera.main;

    _inventory = GetComponent<PlayerInventory>();

    _weaponHoldPoint = GameObject.Find("PlayerWeaponDisplayPoint").transform;
    DisplayCurrentWeapon();
  }

  private void AddBerserkerSpeed(object sender, EventArgs e) {
    if (!_inventory.HasItem(Item.BerserkersFury)) return;
    _berserkerSpeed += 0.05f;
    if (_berserkerSpeed > 0.6f) {
      _berserkerSpeed = 0.6f;
    }

    _tempSpeed = _speed + _berserkerSpeed;
    OnAttackSpeedChange?.Invoke(this, EventArgs.Empty);
    
    if (_isBerserkerDecaying) return;
    StartCoroutine(BerserkerSpeedDecay());
  }

  private IEnumerator BerserkerSpeedDecay() {
    _isBerserkerDecaying = true;
    while (_berserkerSpeed > 0) {
      yield return new WaitForSeconds(0.1f);
      if (_berserkerSpeed > 0.5f) _berserkerSpeed -= 0.004f;
      if (_berserkerSpeed > 0.4f) _berserkerSpeed -= 0.003f;
      if (_berserkerSpeed > 0.3f) _berserkerSpeed -= 0.002f;
      if (_berserkerSpeed > 0.2f) _berserkerSpeed -= 0.002f;
      if (_berserkerSpeed > 0.1f) _berserkerSpeed -= 0.002f;
      _berserkerSpeed -= 0.002f;
      _tempSpeed = _speed + _berserkerSpeed;
      OnAttackSpeedChange?.Invoke(this, EventArgs.Empty);
    }
  
    _berserkerSpeed = 0;
    _tempSpeed = _speed;
    _isBerserkerDecaying = false;
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

      BasePlayerWeapon weapon = _currentWeapon.GetComponent<BasePlayerWeapon>();
      weapon.SetSpeed(GetSpeedMultiplier());
      weapon.MultiplyDamage(_damageMultiplier);

      if (_currentWeaponIndex == 0 && _inventory.HasItem(Item.MasterKnife) && _knifeBeamOffCooldown) {
        ButterKnife knife = weapon as ButterKnife;
        knife.IsKnifeBeam = true;

        StartCoroutine(KnifeBeamCooldown());
        _knifeBeamOffCooldown = false;
      }

      if (_weaponModifiers.Any()) {
        weapon.SetModifiers(_weaponModifiers);
      }
    }
  }

  private void AddModifier(object sender, ElementalItemEventArgs e) {
    _weaponModifiers.Add(e.status);
  }

  private void HandleWeaponAnimEnd(object sender, EventArgs e) {
    DisplayCurrentWeapon();
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
    PlayerControls.OnLeftClick -= HandleAttack;
    ElementalItem.OnElementalItemPickup -= AddModifier;
    BasePlayerWeapon.OnWeaponAnimationEnd -= HandleWeaponAnimEnd;
  }
  
  private void SwitchKnife(object sender, EventArgs e) {
    if (_currentWeaponIndex != 0) {
      _currentWeaponIndex = 0;
      if (_currentWeapon == null) {
        DisplayCurrentWeapon();
      }
      OnSwitchKnife?.Invoke(this, EventArgs.Empty);
    }
  }
  
  private void SwitchFork(object sender, EventArgs e) {
    if (!_isForkUnlocked) return;
    if (_currentWeaponIndex != 1) {
      _currentWeaponIndex = 1;
      if (_currentWeapon == null) {
        DisplayCurrentWeapon();
      }
      OnSwitchFork?.Invoke(this, EventArgs.Empty);
    }
  }
  
  private void SwitchSpoon(object sender, EventArgs e) {
    if (!_isSpoonUnlocked) return;
    if (_currentWeaponIndex != 2) {
      _currentWeaponIndex = 2;
      if (_currentWeapon == null) {
        DisplayCurrentWeapon();
      }
      OnSwitchSpoon?.Invoke(this, EventArgs.Empty);
    }
  }

  private IEnumerator KnifeBeamCooldown() {
    yield return new WaitForSeconds(2 * _speed);

    _knifeBeamOffCooldown = true;
  }

  public void AddToSpeedMultiplier(float additionalSpeed) {
    _speed += additionalSpeed;
    _tempSpeed = _speed;
    OnAttackSpeedChange?.Invoke(this, EventArgs.Empty);
  }

  public void ScaleDamageMultiplier(float scale) {
    _damageMultiplier *= scale;
    OnDamageMultiplierChange?.Invoke(this, EventArgs.Empty);
  }

  public void AddToDamageMultiplier(float additionalDamage) {
    _damageMultiplier += additionalDamage;
    OnDamageMultiplierChange?.Invoke(this, EventArgs.Empty);
  }

  public float GetDamageMultiplier() {
    return _damageMultiplier;
  }
  public float GetSpeedMultiplier() {
    if (Math.Abs(_tempSpeed - _speed) > 0.005f) return _tempSpeed;
    else return _speed;
  }
}
