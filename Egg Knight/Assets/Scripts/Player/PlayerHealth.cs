using System;
using System.Collections;
using UnityEngine;

public class PlayerHealth : Health {
  [SerializeField] private float _iFramesDuration;
  private bool _iFramesActive = false;

  private PlayerInventory _inventory;

  public static event EventHandler OnIFramesEnabled;
  public static event EventHandler OnIFramesDisabled;

  protected override void Awake() {
    _inventory = gameObject.GetComponent<PlayerInventory>();

    PlayerMovement.OnRollBegin += HandleRoll;
    PlayerMovement.OnRollEnd += HandleRollEnd;
    
    base.Awake();
  }

  private void HandleRoll(object sender, RollEventArgs e) {
    DisableHitbox();
  }

  private void HandleRollEnd(object sender, EventArgs e) {
    if (_iFramesActive == false) {
      EnableHitbox();
    }
  }

  private void DisableHitbox() {
    Physics2D.IgnoreLayerCollision((int)Layers.Player, (int)Layers.Enemy);
    Physics2D.IgnoreLayerCollision((int)Layers.Player, (int)Layers.EnemyProjectile);
    Physics2D.IgnoreLayerCollision((int)Layers.Player, (int)Layers.EnemyWeapon);
  }

  private void EnableHitbox() {
    Physics2D.IgnoreLayerCollision((int)Layers.Player, (int)Layers.Enemy, false);
    Physics2D.IgnoreLayerCollision((int)Layers.Player, (int)Layers.EnemyProjectile, false);
    Physics2D.IgnoreLayerCollision((int)Layers.Player, (int)Layers.EnemyWeapon, false);
  }

  public bool BelowHalfHealth() {
    return _currentHealth < (_maxHealth / 2.0f);
  }

  public float CurrentHealthPercentage() {
    return _currentHealth / _maxHealth;
  }

  public bool DamageWillKill(float damage) {
    return (_currentHealth - damage) <= 0;
  }

  public void YolkDamage(float amount) {
    if (DamageWillKill(amount)) {
      return;
    }

    _currentHealth -= amount;
  }

  public override void Damage(float amount) {
    amount = amount - (0.05f * _inventory.GetItemQuantity(Item.BrandNewHelmet) * amount);
    _currentHealth -= amount;

    if (_currentHealth <= 0) {
      Die();
    } else {
      StartCoroutine(IFramesOnHit());
    }
  }

  protected override void Die() {
    if (_inventory.GetItemQuantity(Item.SecondYolk) > 0) {
      _currentHealth = _maxHealth * 0.3f;
      StartCoroutine(IFramesOnHit());

      _inventory.RemoveItem(Item.SecondYolk);

      return;
    }

    Destroy(gameObject);
  }

  private IEnumerator IFramesOnHit() {
    _iFramesActive = true;
    OnIFramesEnabled?.Invoke(this, EventArgs.Empty);

    DisableHitbox();

    yield return new WaitForSeconds(_iFramesDuration);

    EnableHitbox();

    OnIFramesDisabled?.Invoke(this, EventArgs.Empty);
    _iFramesActive = false;
  }
}
