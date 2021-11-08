using System;
using System.Collections;
using UnityEngine;

public abstract class Health : MonoBehaviour {
  [SerializeField] protected float _maxHealth;
  public event EventHandler OnDeath;
  public event EventHandler OnPreDeath;

  protected float _currentHealth;
  public float CurrentHealth {
    get => _currentHealth;
  }

  protected bool _takingCriticalDamage = false;
  public bool TakingCriticalDamage {
    set {
      this._takingCriticalDamage = value;
    }
  }

  [SerializeField] protected GameObject _changeIndicatorPrefab;

  protected virtual void Awake() {
    _currentHealth = _maxHealth;
  }

  public virtual void Damage(float amount) {
    _currentHealth -= amount;

    SpawnChangeIndicator(amount, Color.red);

    if (_currentHealth <= 0) {
      Die();
    }
  }

  public virtual void Heal(float amount) {
    _currentHealth += amount;

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
}
