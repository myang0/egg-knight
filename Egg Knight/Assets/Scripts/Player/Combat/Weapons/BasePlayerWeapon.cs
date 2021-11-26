using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasePlayerWeapon : MonoBehaviour {
  protected Animator _anim;
  protected SpriteRenderer _sr;

  protected List<StatusCondition> _weaponModifiers;

  protected GameObject _playerObject;

  protected PlayerHealth _health;
  protected PlayerInventory _inventory;
  protected PlayerCursedInventory _cursedInventory;
  protected PlayerWallet _wallet;

  [SerializeField] protected float _damageAmount;
  [SerializeField] protected DamageType _damageType;

  [SerializeField] protected LayerMask _enemyLayer;
  [SerializeField] protected LayerMask _coinLayer;

  public static event EventHandler OnWeaponAnimationEnd;

  protected virtual void Awake() {
    _anim = gameObject.GetComponent<Animator>();
    _sr = gameObject.GetComponent<SpriteRenderer>();

    _weaponModifiers = new List<StatusCondition>();
    
    _playerObject = GameObject.FindGameObjectWithTag("Player");

    _health = _playerObject.GetComponent<PlayerHealth>();
    _inventory = _playerObject.GetComponent<PlayerInventory>();
    _cursedInventory = _playerObject.GetComponent<PlayerCursedInventory>();
    _wallet = _playerObject.GetComponent<PlayerWallet>();

    Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    _sr.flipX = (mousePos.x < transform.position.x);
  }

  public void SetModifiers(List<StatusCondition> modifiers) {
    foreach (StatusCondition modifier in modifiers) {
      _weaponModifiers.Add(modifier);
    }
  }

  protected virtual void FixedUpdate() {
    Vector3 playerPos = _playerObject.transform.position;
    Vector3 newPos = new Vector3(playerPos.x, playerPos.y, ZcoordinateConsts.WeaponAttack);
    transform.position = newPos;
  }

  protected void DamageEnemies(Collider2D[] enemies) {
    GameObject[] enemyObjects = enemies.Select(e => e.gameObject).ToArray();
    GameObject[] uniqueEnemyObjects = enemyObjects.Distinct().ToArray();

    foreach (GameObject enemyObject in uniqueEnemyObjects) {
      EnemyHealth enemyHealth = enemyObject.GetComponent<EnemyHealth>();

      if (enemyHealth != null && !enemyHealth.isInvulnerable) {
        HealOnHit();

        DamageEnemy(enemyHealth);
      }

      if (enemyHealth != null && enemyHealth.isInvulnerable) {
        enemyHealth.Damage(0);
      }
    }
  }

  protected void CollectCoins(Collider2D[] pickups) {
    foreach (Collider2D p in pickups) {
      PickupBase pickup = p.gameObject.GetComponent<PickupBase>();
      if (pickup.allowWeaponPickup) pickup.PickUp(_playerObject);
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

    float amountAfterProtein = AddProteinDamage(_damageAmount);
    float amountAfterCoins = AddCoinDamage(amountAfterProtein);
    float amountAfterRage = AddRageDamage(amountAfterCoins);
    float totalAmount = HandleCrits(amountAfterRage, statuses, enemyHealth);

    if (statuses.Any()) {
      enemyHealth.DamageWithStatusesAndType(totalAmount, statuses, _damageType);
    } else {
      enemyHealth.DamageWithType(totalAmount, _damageType);
    }
  }

  protected virtual float AddProteinDamage(float originalAmount) {
    return originalAmount * (_inventory.HasItem(Item.ProteinPowder) ? 1.2f : 1.0f);
  }

  protected virtual float AddCoinDamage(float originalAmount) {
    float totalAmount = originalAmount;

    if (_inventory.HasItem(Item.GoldChainNecklace)) {
      totalAmount += (originalAmount * _wallet.GetBalance() * 0.02f);
    }

    return totalAmount;
  }

  protected virtual float AddRageDamage(float originalAmount) {
    float totalAmount = originalAmount;

    if (_health.BelowHalfHealth() && _inventory.HasItem(Item.VikingHelmet)) {
      totalAmount += (0.5f - _health.CurrentHealthPercentage()) * _inventory.GetItemQuantity(Item.VikingHelmet) * totalAmount;
    }

    return totalAmount;
  }

  protected virtual float HandleCrits(float originalAmount, List<StatusCondition> _statuses, EnemyHealth eHealth) {
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

      eHealth.TakingCriticalDamage = true;
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
