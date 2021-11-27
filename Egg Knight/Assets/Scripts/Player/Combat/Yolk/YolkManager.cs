using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YolkManager : MonoBehaviour {
  [SerializeField] private GameObject _yolkPrefab;

  [SerializeField] private float _yolkCooldown = 1.0f;
  private bool _yolkOffCooldown = true;

  [SerializeField] private float _maxYolk = 90;
  private float _currentYolk;

  [SerializeField] private float _yolkCost = 30;
  [SerializeField] private float _yolkRegenPerSecond = 10;

  private float _speedScaling = 1.0f;
  private float _damageScaling = 1.0f;

  private YolkUpgradeManager _upgrades;
  private PlayerCursedInventory _cursedInventory;

  public static event EventHandler<YolkChangeEventArgs> OnYolkChange;

  private void Awake() {
    PlayerControls.OnRightClick += HandleRightClick;

    _currentYolk = _maxYolk;

    _upgrades = gameObject.GetComponent<YolkUpgradeManager>();
    _cursedInventory = gameObject.GetComponent<PlayerCursedInventory>();

    StartCoroutine(YolkRegen());
  }

  private void HandleRightClick(object sender, EventArgs e) {
    if (_yolkOffCooldown) {
      if (_yolkCost > _currentYolk) {
        return;
      }

      _currentYolk -= _yolkCost;
      OnYolkChange?.Invoke(this, new YolkChangeEventArgs(CurrentYolkPercent()));

      StartCoroutine(ShootYolk());
    }
  }

  IEnumerator ShootYolk() {
    _yolkOffCooldown = false;

    Vector3 mousePosInWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    Vector2 vectorToMouse = VectorHelper.GetVectorToPoint(transform.position, mousePosInWorld);

    float angleToMouse = Vector2.SignedAngle(Vector2.up, vectorToMouse);

    if (_upgrades.HasUpgrade(YolkUpgradeType.LeghornShot)) {
      SpawnYolk(Quaternion.Euler(0, 0, -10) * vectorToMouse, angleToMouse - 10);
      SpawnYolk(vectorToMouse, angleToMouse);
      SpawnYolk(Quaternion.Euler(0, 0, 10) * vectorToMouse, angleToMouse + 10);
    } else {
      SpawnYolk(vectorToMouse, angleToMouse);
    }

    yield return new WaitForSeconds(_yolkCooldown);

    _yolkOffCooldown = true;
  }

  private IEnumerator YolkRegen() {
    while (true) {
      yield return new WaitForSeconds(0.05f);

      float lastYolk = _currentYolk;
      _currentYolk = (_currentYolk + (_yolkRegenPerSecond / 20f) < _maxYolk) ? _currentYolk + (_yolkRegenPerSecond / 20f) : _maxYolk;

      if (lastYolk != _currentYolk) {
        OnYolkChange?.Invoke(this, new YolkChangeEventArgs(CurrentYolkPercent()));
      }
    }
  }

  private void SpawnYolk(Vector2 direction, float angle) {
    GameObject yolkObject = Instantiate(_yolkPrefab, transform.position, Quaternion.identity);
    YolkProjectile yolk = yolkObject.GetComponent<YolkProjectile>();

    yolk.MultiplySpeed(_speedScaling);
    yolk.SetDirection(direction, angle);
    yolk.MultiplyDamage(_damageScaling);
  }

  private float CurrentYolkPercent() {
    return _currentYolk / _maxYolk;
  }

  public void MultiplyByCooldown(float multiplier) {
    _yolkCooldown *= multiplier;
  }

  public void MultiplyByCost(float multiplier) {
    _yolkCost *= multiplier;
  }

  public void MultiplyBySpeedScaling(float multiplier) {
    _speedScaling *= multiplier;
  }

  public void MultiplyByDamageScaling(float multiplier) {
    _damageScaling *= multiplier;
    StatusConfig.SalmonellaDamage *= multiplier;
  }

  private void OnDestroy() {
    PlayerControls.OnRightClick -= HandleRightClick;
  }
}
