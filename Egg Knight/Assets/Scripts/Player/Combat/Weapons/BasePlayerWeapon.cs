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
  protected PlayerCursedInventory _cursedInventory;
  protected PlayerWallet _wallet;

  [SerializeField] protected float _damageAmount;
  [SerializeField] protected DamageType _damageType;

  public static event EventHandler OnWeaponAnimationEnd;

  protected virtual void Awake() {
    _anim = gameObject.GetComponent<Animator>();
    _sr = gameObject.GetComponent<SpriteRenderer>();

    _weaponModifiers = new List<StatusCondition>();
    
    GameObject player = GameObject.FindGameObjectWithTag("Player");

    _health = player.GetComponent<PlayerHealth>();
    _inventory = player.GetComponent<PlayerInventory>();
    _cursedInventory = player.GetComponent<PlayerCursedInventory>();
    _wallet = player.GetComponent<PlayerWallet>();

    Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    _sr.flipX = (mousePos.x < transform.position.x);
  }

  public void SetModifiers(List<StatusCondition> modifiers) {
    foreach (StatusCondition modifier in modifiers) {
      _weaponModifiers.Add(modifier);
    }
  }

  protected virtual void FixedUpdate() {
    Vector3 playerPos = GameObject.Find("Player").transform.position;
    Vector3 newPos = new Vector3(playerPos.x, playerPos.y, 1);
    transform.position = newPos;
  }

  protected void DamageEnemies(Collider2D[] enemies) {
    GameObject[] enemyObjects = enemies.Select(e => e.gameObject).ToArray();
    GameObject[] uniqueEnemyObjects = enemyObjects.Distinct().ToArray();

    foreach (GameObject enemyObject in uniqueEnemyObjects) {
      EnemyHealth enemyHealth = enemyObject.GetComponent<EnemyHealth>();

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
      int randomNum = UnityEngine.Random.Range(0, 100);
      
      if (randomNum < StatusConfig.StatusEffectChance && !statuses.Contains(modifier)) {
        statuses.Add(modifier);
      }
    }

    float amountAfterCoins = AddCoinDamage(_damageAmount);
    float amountAfterRage = AddRageDamage(amountAfterCoins);
    float totalAmount = HandleCrits(amountAfterRage, statuses);

    if (statuses.Any()) {
      enemyHealth.DamageWithStatusesAndType(totalAmount, statuses, _damageType);
    } else {
      enemyHealth.DamageWithType(totalAmount, _damageType);
    }
  }

  protected virtual float AddCoinDamage(float originalAmount) {
    float totalAmount = originalAmount;

    if (_inventory.ItemInInventory(Item.GoldChainNecklace)) {
      totalAmount += (originalAmount * _wallet.GetBalance() * 0.02f);
    }

    return totalAmount;
  }

  protected virtual float AddRageDamage(float originalAmount) {
    float totalAmount = originalAmount;

    if (_health.BelowHalfHealth() && _inventory.ItemInInventory(Item.VikingHelmet)) {
      totalAmount += (0.5f - _health.CurrentHealthPercentage()) * _inventory.GetItemQuantity(Item.VikingHelmet) * totalAmount;
    }

    return totalAmount;
  }

  protected virtual float HandleCrits(float originalAmount, List<StatusCondition> _statuses) {
    float totalAmount = originalAmount;

    int critRoll = UnityEngine.Random.Range(0, 100);
    int critChance = (10 * _inventory.GetItemQuantity(Item.ThirdEye)) +
      (_cursedInventory.HasItem(CursedItemType.RustySword) ? 25 : 0);

    if (critRoll < critChance) {
      totalAmount *= 2;

      if (_cursedInventory.HasItem(CursedItemType.RustySword)) {
        _health.RustySwordDamage();
        _statuses.Add(StatusCondition.Bleeding);
      }
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
