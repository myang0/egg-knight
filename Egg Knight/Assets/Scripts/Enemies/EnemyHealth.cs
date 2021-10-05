using UnityEngine;

public abstract class EnemyHealth : Health {
  [SerializeField] protected DamageType _weakTo;

  public abstract void DamageWithStatus(float amount, StatusCondition status);

  public abstract void DamageWithType(float amount, DamageType type);
}