using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YolkManager : MonoBehaviour {
  [SerializeField] private Transform _shootPoint;
  [SerializeField] private GameObject _yolkPrefab;

  [SerializeField] private float _yolkCooldown = 1.0f;
  private bool _yolkOffCooldown = true;

  [SerializeField] private float _initialMaxYolk = 90;
  private float _maxYolk;
  private float _currentYolk;

  private bool _maxYolkIncreased = false;

  [SerializeField] private float _yolkCost = 30;

  [SerializeField] private float _initialRegenPerSecond = 10f;
  private float _regenPerSecond;

  private float _speedScaling = 1.0f;

  private float _damageScaling = 1.0f;
  public float DamageScaling {
    get {
      return _damageScaling;
    }
  }

  private YolkUpgradeManager _upgrades;
  private PlayerCursedInventory _cursedInventory;
  private SoundPlayer _soundPlayer;

  [SerializeField] private AudioClip _shootClip;

  public static event EventHandler<YolkChangeEventArgs> OnYolkChange;

  private void Awake() {
    PlayerControls.OnRightClick += HandleRightClick;

    _maxYolk = _initialMaxYolk;
    _currentYolk = _maxYolk;

    _regenPerSecond = _initialRegenPerSecond;

    _upgrades = GetComponent<YolkUpgradeManager>();
    _cursedInventory = GetComponent<PlayerCursedInventory>();
    _soundPlayer = GetComponent<SoundPlayer>();

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
      _currentYolk = (_currentYolk + (_regenPerSecond / 20f) < _maxYolk) ? _currentYolk + (_regenPerSecond / 20f) : _maxYolk;

      if (lastYolk != _currentYolk) {
        OnYolkChange?.Invoke(this, new YolkChangeEventArgs(CurrentYolkPercent()));
      }
    }
  }

  private void SpawnYolk(Vector2 direction, float angle) {
    GameObject yolkObject = Instantiate(_yolkPrefab, _shootPoint.position, Quaternion.identity);
    YolkProjectile yolk = yolkObject.GetComponent<YolkProjectile>();

    yolk.MultiplySpeed(_speedScaling);
    yolk.SetDirection(direction, angle);
    yolk.MultiplyDamage(_damageScaling);

    _soundPlayer.PlayClip(_shootClip, 1.5f);
  }

  private float CurrentYolkPercent() {
    return _currentYolk / _maxYolk;
  }

  public void MultiplyByCooldown(float multiplier) {
    _yolkCooldown *= multiplier;
  }

  public void MultiplyByCost(float multiplier) {
    _yolkCost *= multiplier;

    if (_yolkCost > _maxYolk) {
      ScaleMaxYolkAndRegen(_yolkCost);

      _maxYolkIncreased = true;
    } else if (_yolkCost < _maxYolk && _maxYolkIncreased) {
      if (_yolkCost < _initialMaxYolk) {
        _maxYolk = _initialMaxYolk;
        _regenPerSecond = _initialRegenPerSecond;

        _maxYolkIncreased = false;
      } else {
        ScaleMaxYolkAndRegen(_yolkCost);
      }
    }
  }

  private void ScaleMaxYolkAndRegen(float newCost) {
    float prevMaxYolk = _maxYolk;
    _maxYolk = newCost;

    float percentageChange = _maxYolk / prevMaxYolk;
    _regenPerSecond *= percentageChange;
  }

  public void MultiplyBySpeedScaling(float multiplier) {
    _speedScaling *= multiplier;
  }

  public void MultiplyByDamageScaling(float multiplier) {
    _damageScaling *= multiplier;
  }

  private void OnDestroy() {
    PlayerControls.OnRightClick -= HandleRightClick;
  }
}
