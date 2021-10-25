using System;
using UnityEngine;

public abstract class Health : MonoBehaviour {
  [SerializeField] protected float _maxHealth;
  public event EventHandler OnDeath;
  protected float _currentHealth;

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
    OnDeath?.Invoke(this, EventArgs.Empty);
    Destroy(gameObject);
  }

  protected void SpawnChangeIndicator(float value, Color color) {
    if (_changeIndicatorPrefab != null) {
      GameObject changeIndicatorObject = Instantiate(_changeIndicatorPrefab, transform.position, Quaternion.identity);
      HealthChangeIndicator changeIndicator = changeIndicatorObject?.GetComponent<HealthChangeIndicator>();

      changeIndicator?.Initialize(value, color);
    }
  }
}
