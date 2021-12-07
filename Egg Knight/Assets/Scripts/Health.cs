using System;
using System.Collections;
using UnityEngine;

public abstract class Health : MonoBehaviour {
  [SerializeField] public float _maxHealth;
  public event EventHandler OnDeath;
  public event EventHandler OnPreDeath;

  public float _currentHealth;
  public bool isInvulnerable;
  public float CurrentHealth {
    get => _currentHealth;
  }

  protected bool _takingCriticalDamage = false;
  public bool TakingCriticalDamage {
    set {
      this._takingCriticalDamage = value;
    }
  }

  protected float _initialArmourValue = 1.0f;
  protected float _armourValue = 1.0f;

  [SerializeField] protected GameObject _changeIndicatorPrefab;

  protected virtual void Awake() {
    _armourValue = _initialArmourValue;

    _currentHealth = _maxHealth;
  }

  public virtual void Damage(float amount) {
    if (_currentHealth <= 0) {
      return;
    }

    if (isInvulnerable) {
      SoundManager.Instance.PlaySound(Sound.Block, volumeScaling: 0.5f);
      amount = 0;
    }

    if (_armourValue == 0) {
      Debug.LogError("Armour value was zero");
      _armourValue = 1.0f;
    }

    amount *= (1.0f / _armourValue);
    _currentHealth -= amount;

    SpawnChangeIndicator(amount, Color.red);

    if (_currentHealth <= 0) {
      Die();
    }
  }

  public virtual void Heal(float amount) {
    _currentHealth += Mathf.RoundToInt(amount);

    SpawnChangeIndicator(amount, Color.green);

    if (_currentHealth > _maxHealth) {
      _currentHealth = _maxHealth;
    }
  }

  protected virtual void Die() {
    OnPreDeath?.Invoke(this, EventArgs.Empty);
    StartCoroutine(DelayedDeath());
    // OnDeath?.Invoke(this, EventArgs.Empty);
    // Destroy(gameObject);
  }

  private IEnumerator DelayedDeath() {
    yield return new WaitForSeconds(1.5f);
    OnDeath?.Invoke(this, EventArgs.Empty);
    Destroy(gameObject);
  }

  protected void SpawnChangeIndicator(float value, Color color) {
    if (_changeIndicatorPrefab != null) {
      GameObject changeIndicatorObject = Instantiate(_changeIndicatorPrefab, transform.position, Quaternion.identity);
      HealthChangeIndicator changeIndicator = changeIndicatorObject?.GetComponent<HealthChangeIndicator>();

      if (_takingCriticalDamage) {
        changeIndicator?.InitializeCritical(value);
      } else {
        changeIndicator?.Initialize(value, color);
      }

      _takingCriticalDamage = false;
    }
  }

  public float CurrentHealthPercentage() {
    return ((_currentHealth / _maxHealth) < 0) ? 0 : (_currentHealth / _maxHealth);
  }

  public virtual void AddToMaxHealth(float addValue) {
    float origPercent = CurrentHealthPercentage();

    _maxHealth += addValue;
    _currentHealth = _maxHealth * origPercent;
  }

  public float GetArmour() {
    return _armourValue;
  }
}
