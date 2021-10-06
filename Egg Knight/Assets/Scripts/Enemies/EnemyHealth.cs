using UnityEngine;

public abstract class EnemyHealth : Health {
  [SerializeField] protected DamageType _weakTo;

  public abstract void DamageWithStatus(float amount, StatusCondition status);

  public virtual void DamageWithType(float amount, DamageType type) {
    float bonusDamage = (type == _weakTo) ? (amount * 0.5f) : 0;
    Damage(amount + bonusDamage);
  }
}