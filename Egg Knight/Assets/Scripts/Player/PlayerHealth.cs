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
  private SoundPlayer _soundPlayer;

  [SerializeField] private AudioClip _healClip;
  [SerializeField] private AudioClip _damageClip;
  [SerializeField] private AudioClip _ninjaClip;
  public GameObject secondYolkAura;
  private float _healMultiplier = 1.0f;

  public static event EventHandler<HealthChangeEventArgs> OnHealthDecrease;
  public static event EventHandler<HealthChangeEventArgs> OnHealthIncrease;

  public static event EventHandler<PlayerHealthChangeEventArgs> OnHealthChange;

  public static event EventHandler OnIFramesEnabled;
  public static event EventHandler OnIFramesDisabled;

  [SerializeField] private GameObject _ninjaParticles;
  [SerializeField] private float _ninjaInvincibilityTime;

  public static event EventHandler OnNinjaIFramesEnabled;
  public static event EventHandler OnNinjaIFramesDisabled;

  public static event EventHandler OnGameOver;
  public static event EventHandler OnReviveGameOver;
  public static event EventHandler OnRevive;
  public bool isDead;

  protected override void Awake() {
    _inventory = GetComponent<PlayerInventory>();
    _cursedInventory = GetComponent<PlayerCursedInventory>();
    _soundPlayer = GetComponent<SoundPlayer>();

    PlayerDeath.OnReviveSequenceDone += SecondYolkEffectSubscriber;
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

    if (_armourValue == 0) {
      _armourValue = 1;
    }

    amount /= _armourValue;
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
    Debug.Log("AA");
    
    if (_inventory.GetItemQuantity(Item.SecondYolk) > 0) {
      OnReviveGameOver?.Invoke(this, EventArgs.Empty);
      return;
    }

    if (!isDead) {
      isDead = true;
      OnGameOver?.Invoke(this, EventArgs.Empty);
    }
  }

  private void SecondYolkEffectSubscriber(object sender, EventArgs e) {
    StartCoroutine(SecondYolkEffect());
  }

  private IEnumerator SecondYolkEffect() {
    Heal(_maxHealth * 0.5f);
    _inventory.RemoveItem(Item.SecondYolk);
    secondYolkAura.SetActive(true);
    _iFramesActive = true;
    DisableHitbox();
    
    float effectDuration = 3f;
    while (effectDuration > 0) {
      effectDuration -= Time.deltaTime;
      secondYolkAura.transform.Rotate(0, 0, 360*Time.deltaTime);
      yield return new WaitForFixedUpdate();
    }
    
    EnableHitbox();
    secondYolkAura.SetActive(false);
    _iFramesActive = false;
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
    amount *= _healMultiplier;

    base.Heal(amount);

    _soundPlayer.PlayClip(_healClip, 2f);

    OnHealthIncrease?.Invoke(this, new HealthChangeEventArgs(CurrentHealthPercentage()));
    OnHealthChange?.Invoke(this, new PlayerHealthChangeEventArgs(_currentHealth, (int)_maxHealth));
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
    PlayerDeath.OnReviveSequenceDone -= SecondYolkEffectSubscriber;
  }

  public void ScaleArmour(float scale) {
    _armourValue *= scale;
  }

  public void ScaleHealMultiplier(float scale) {
    _healMultiplier *= scale;
  }
}
