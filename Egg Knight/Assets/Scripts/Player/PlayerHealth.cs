using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : Health {
  [SerializeField] private float _iFramesDuration;
  private bool _iFramesActive = false;

  private bool _isRolling = false;

  private PlayerInventory _inventory;
  private PlayerCursedInventory _cursedInventory;

  public static event EventHandler<HealthChangeEventArgs> OnHealthDecrease;
  public static event EventHandler<HealthChangeEventArgs> OnHealthIncrease;

  public static event EventHandler OnIFramesEnabled;
  public static event EventHandler OnIFramesDisabled;

  protected override void Awake() {
    _inventory = gameObject.GetComponent<PlayerInventory>();
    _cursedInventory = gameObject.GetComponent<PlayerCursedInventory>();

    PlayerMovement.OnRollBegin += HandleRoll;
    PlayerMovement.OnRollEnd += HandleRollEnd;
    
    base.Awake();
  }

  private void HandleRoll(object sender, RollEventArgs e) {
    _isRolling = true;

    DisableHitbox();
  }

  private void HandleRollEnd(object sender, EventArgs e) {
    _isRolling = false;

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

  public bool DamageWillKill(float damage) {
    return (_currentHealth - damage) <= 0;
  }

  public void YolkDamage(float amount) {
    if (DamageWillKill(amount)) {
      return;
    }

    SpawnChangeIndicator(amount, Color.yellow);

    _currentHealth -= amount;

    OnHealthDecrease?.Invoke(this, new HealthChangeEventArgs(CurrentHealthPercentage()));
  }

  public override void Damage(float amount) {
    if (_iFramesActive || _isRolling) {
      return;
    }

    amount = amount - (0.1f * _inventory.GetItemQuantity(Item.BrandNewHelmet) * amount);
    _currentHealth -= amount;

    SpawnChangeIndicator(amount, Color.red);

    OnHealthDecrease?.Invoke(this, new HealthChangeEventArgs(CurrentHealthPercentage()));

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

    SceneManager.LoadScene(2);

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

  public override void Heal(float amount) {
    float trueAmount = _cursedInventory.HasItem(CursedItemType.RottenYolk) ? amount / 2.0f : amount;

    base.Heal(trueAmount);

    OnHealthIncrease?.Invoke(this, new HealthChangeEventArgs(CurrentHealthPercentage()));
  }

  public void RustySwordDamage() {
    if (CurrentHealthPercentage() >= 0.2f) {
      _currentHealth -= 1;

      SpawnChangeIndicator(1, Color.red);

      OnHealthDecrease?.Invoke(this, new HealthChangeEventArgs(CurrentHealthPercentage()));
    }
  }

  private void OnDestroy() {
    PlayerMovement.OnRollBegin -= HandleRoll;
    PlayerMovement.OnRollEnd -= HandleRollEnd;
  }
}
