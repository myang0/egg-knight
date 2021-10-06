using UnityEngine;

public abstract class Health : MonoBehaviour {
  [SerializeField] protected float _maxHealth;
  protected float _currentHealth;

  protected virtual void Awake() {
    _currentHealth = _maxHealth;
  }

  public virtual void Damage(float amount) {
    _currentHealth -= amount;

    if (_currentHealth <= 0) {
      Die();
    }
  }

  public virtual void Heal(float amount) {
    _currentHealth += amount;

    if (_currentHealth > _maxHealth) {
      _currentHealth = _maxHealth;
    }
  }

  protected virtual void Die() {
    Destroy(gameObject);
  }
}
