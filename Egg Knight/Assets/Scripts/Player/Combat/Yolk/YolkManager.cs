using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YolkManager : MonoBehaviour {
  [SerializeField] private GameObject _yolkPrefab;

  [SerializeField] private float _yolkCooldown = 2.0f;
  private bool _yolkOffCooldown = true;

  [SerializeField] private float _yolkHealthCost = 2.5f;
  private PlayerHealth _health;

  private YolkUpgradeManager _upgrades;
  private PlayerCursedInventory _cursedInventory;

  private void Awake() {
    PlayerControls.OnRightClick += HandleRightClick;

    _health = gameObject.GetComponent<PlayerHealth>();

    _upgrades = gameObject.GetComponent<YolkUpgradeManager>();
    _cursedInventory = gameObject.GetComponent<PlayerCursedInventory>();
  }

  private void HandleRightClick(object sender, EventArgs e) {
    if (_yolkOffCooldown) {
      if (_health.DamageWillKill(_yolkHealthCost)) {
        return;
      }

      _health.YolkDamage(_yolkHealthCost);
      StartCoroutine(ShootYolk());
    }
  }

  IEnumerator ShootYolk() {
    _yolkOffCooldown = false;

    Vector3 mousePosInWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    Vector2 vectorToMouse = VectorHelper.GetVectorToPoint(transform.position, mousePosInWorld);

    float angleToMouse = Vector2.SignedAngle(Vector2.up, vectorToMouse);

    if (_upgrades.HasUpgrade(YolkUpgradeType.LeghornShot)) {
      SpawnYolk(Quaternion.Euler(0, 0, -20) * vectorToMouse, angleToMouse - 20);
      SpawnYolk(vectorToMouse, angleToMouse);
      SpawnYolk(Quaternion.Euler(0, 0, 20) * vectorToMouse, angleToMouse + 20);
    } else {
      SpawnYolk(vectorToMouse, angleToMouse);
    }

    yield return new WaitForSeconds(_yolkCooldown);

    _yolkOffCooldown = true;
  }

  private void SpawnYolk(Vector2 direction, float angle) {
    GameObject yolkObject = Instantiate(_yolkPrefab, transform.position, Quaternion.identity);
    YolkProjectile yolk = yolkObject.GetComponent<YolkProjectile>();

    yolk.SetDirection(direction, angle);
    if (_cursedInventory.HasItem(CursedItemType.RottenYolk)) {
      yolk.MultiplyDamage(2.0f);
    }
  }

  public void MultiplyByCooldown(float multiplier) {
    _yolkCooldown *= multiplier;
  }

  public void MultiplyByHealthCost(float multiplier) {
    _yolkHealthCost *= multiplier;
  }
}
