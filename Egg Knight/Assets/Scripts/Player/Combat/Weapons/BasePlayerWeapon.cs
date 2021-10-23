using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasePlayerWeapon : MonoBehaviour {
  protected Animator _anim;
  protected SpriteRenderer _sr;

  protected List<StatusCondition> _weaponModifiers;

  protected PlayerHealth _health;
  protected PlayerInventory _inventory;

  [SerializeField] protected float _damageAmount;
  [SerializeField] protected DamageType _damageType;

  public static event EventHandler OnWeaponAnimationEnd;

  protected void Awake() {
    _anim = gameObject.GetComponent<Animator>();
    _sr = gameObject.GetComponent<SpriteRenderer>();

    _weaponModifiers = new List<StatusCondition>();

    GameObject player = GameObject.FindGameObjectWithTag("Player");
    _health = player.GetComponent<PlayerHealth>();
    _inventory = player.GetComponent<PlayerInventory>();

    Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    _sr.flipX = (mousePos.x < transform.position.x);
  }

  public void SetModifiers(List<StatusCondition> modifiers) {
    foreach (StatusCondition modifier in modifiers) {
      _weaponModifiers.Add(modifier);
    }
  }

  protected virtual void FixedUpdate() {
    Transform player = GameObject.Find("Player").transform;

    transform.position = player.position;
  }

  protected void DamageEnemies(Collider2D[] enemies) {
    foreach (Collider2D enemy in enemies) {
      EnemyHealth enemyHealth = enemy.gameObject.GetComponent<EnemyHealth>();

      if (enemyHealth != null) {
        HealOnHit();

        DamageEnemy(enemyHealth);
      }
    }
  }

  protected virtual void HealOnHit() {
    int healRoll = UnityEngine.Random.Range(0, 100);

    if (healRoll < (25 * _inventory.GetItemQuantity(Item.VampireFangs))) {
      _health.Heal(1);
    }
  }

  protected virtual void DamageEnemy(EnemyHealth enemyHealth) {
    List<StatusCondition> statuses = new List<StatusCondition>();

    foreach (StatusCondition modifier in _weaponModifiers) {
      int randomNum = UnityEngine.Random.Range(0, 1);
      
      if (randomNum < StatusConfig.StatusEffectChance && !statuses.Contains(modifier)) {
        statuses.Add(modifier);
      }
    }

    float totalAmount = ModifyDamage(_damageAmount);

    if (statuses.Any()) {
      enemyHealth.DamageWithStatusesAndType(totalAmount, statuses, _damageType);
    } else {
      enemyHealth.DamageWithType(totalAmount, _damageType);
    }
  }

  protected virtual float ModifyDamage(float originalAmount) {
    float totalAmount = originalAmount;

    if (_health.BelowHalfHealth() && _inventory.ItemInInventory(Item.VikingHelmet)) {
      totalAmount += (0.5f - _health.CurrentHealthPercentage()) * _inventory.GetItemQuantity(Item.VikingHelmet) * totalAmount;
    }

    int critRoll = UnityEngine.Random.Range(0, 100);

    if (critRoll < 10 * _inventory.GetItemQuantity(Item.ThirdEye)) {
      totalAmount *= 2;
    }

    return totalAmount;
  }

  public abstract void EnableHitbox();

  public virtual void OnAnimationEnd() {
    Destroy(gameObject);

    OnWeaponAnimationEnd?.Invoke(this, EventArgs.Empty);
  }

  protected abstract void OnDrawGizmosSelected();
}
