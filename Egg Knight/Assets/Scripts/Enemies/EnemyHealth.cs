using UnityEngine;

public abstract class EnemyHealth : Health {
  [SerializeField] protected DamageType _weakTo;

  public abstract void Damage(float amount, DamageType type);
}