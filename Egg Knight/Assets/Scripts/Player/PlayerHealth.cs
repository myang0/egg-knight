using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class PlayerHealth : Health {
  [SerializeField] private float _iFramesDuration;
  private bool _iFramesActive = false;

  private bool _isRolling = false;

  private PlayerInventory _inventory;
  private PlayerCursedInventory _cursedInventory;

  public static event EventHandler<HealthChangeEventArgs> OnHealthDecrease;
  public static event EventHandler<HealthChangeEventArgs> OnHealthIncrease;

  public static event EventHandler<PlayerHealthChangeEventArgs> OnHealthChange;

  public static event EventHandler OnIFramesEnabled;
  public static event EventHandler OnIFramesDisabled;

  [SerializeField] private GameObject _ninjaParticles;
  [SerializeField] private float _ninjaInvincibilityTime;

  public static event EventHandler OnNinjaIFramesEnabled;
  public static event EventHandler OnNinjaIFramesDisabled;

  protected override void Awake() {
    _inventory = gameObject.GetComponent<PlayerInventory>();
    _cursedInventory = gameObject.GetComponent<PlayerCursedInventory>();

    PlayerMovement.OnRollBegin += HandleRoll;
    PlayerMovement.OnRollEnd += HandleRollEnd;
    
    base.Awake();

    OnHealthChange?.Invoke(this, new PlayerHealthChangeEventArgs(_currentHealth, (int)_maxHealth));
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
    return _currentHealth <= (_maxHealth / 2.0f);
  }

  public bool DamageWillKill(float damage) {
    return (_currentHealth - damage) <= 0;
  }

  public override void Damage(float amount) {
    if (_iFramesActive || _isRolling) {
      return;
    }

    int ninjaRoll = Random.Range(0, 8);
    if (ninjaRoll < _inventory.GetItemQuantity(Item.NinjaHeadband)) {
      Instantiate(_ninjaParticles, transform.position, Quaternion.identity);
      StartCoroutine(NinjaIFrames());
      return;
    }

    amount = amount - (0.1f * _inventory.GetItemQuantity(Item.BrandNewHelmet) * amount);
    _currentHealth -= amount;

    SpawnChangeIndicator(amount, Color.red);

    OnHealthDecrease?.Invoke(this, new HealthChangeEventArgs(CurrentHealthPercentage()));
    OnHealthChange?.Invoke(this, new PlayerHealthChangeEventArgs(_currentHealth, (int)_maxHealth));

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

    SceneManager.LoadScene(3);

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

  private IEnumerator NinjaIFrames() {
    _iFramesActive = true;
    OnNinjaIFramesEnabled?.Invoke(this, EventArgs.Empty);

    DisableHitbox();

    yield return new WaitForSeconds(_ninjaInvincibilityTime);

    OnNinjaIFramesDisabled?.Invoke(this, EventArgs.Empty);

    if (_isRolling == false) {
      EnableHitbox();
      _iFramesActive = false;
    }
  }

  public override void Heal(float amount) {
    float trueAmount = _cursedInventory.HasItem(CursedItemType.RottenYolk) ? amount / 2.0f : amount;

    base.Heal(trueAmount);

    OnHealthIncrease?.Invoke(this, new HealthChangeEventArgs(CurrentHealthPercentage()));
    OnHealthChange?.Invoke(this, new PlayerHealthChangeEventArgs((int)Mathf.Ceil(_currentHealth), (int)_maxHealth));
  }

  public override void AddToMaxHealth(float addValue) {
    base.AddToMaxHealth(addValue);

    OnHealthChange?.Invoke(this, new PlayerHealthChangeEventArgs(_currentHealth, (int)_maxHealth));
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
