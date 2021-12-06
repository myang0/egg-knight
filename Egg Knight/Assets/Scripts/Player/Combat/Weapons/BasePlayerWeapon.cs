using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasePlayerWeapon : MonoBehaviour {
  protected Animator _anim;
  protected SpriteRenderer _sr;

  protected Camera _mainCamera;

  protected List<StatusCondition> _weaponModifiers;

  protected float _speed = 1.0f;

  protected GameObject _playerObject;

  protected PlayerHealth _health;
  protected PlayerInventory _inventory;
  protected PlayerCursedInventory _cursedInventory;
  protected PlayerWallet _wallet;

  [SerializeField] protected float _damageAmount;
  [SerializeField] protected DamageType _damageType;

  protected float _modifier;

  [SerializeField] protected LayerMask _enemyLayer;
  [SerializeField] protected LayerMask _coinLayer;

  [SerializeField] protected GameObject _singleTimeSound;

  public static event EventHandler OnWeaponAnimationEnd;

  protected virtual void Awake() {
    _anim = GetComponent<Animator>();
    _sr = GetComponent<SpriteRenderer>();

    _mainCamera = Camera.main;

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

  protected void DamageEnemies(Collider2D[] enemies, float damage) {
    GameObject[] enemyObjects = enemies.Select(e => e.gameObject).ToArray();
    GameObject[] uniqueEnemyObjects = enemyObjects.Distinct().ToArray();

    foreach (GameObject enemyObject in uniqueEnemyObjects) {
      EnemyHealth enemyHealth = enemyObject.GetComponent<EnemyHealth>();

      if (enemyHealth != null && !enemyHealth.isInvulnerable) {
        if (DamageEnemy(enemyHealth, damage)) HealOnHit();
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

  protected virtual bool HealOnHit() {
    int healRoll = UnityEngine.Random.Range(0, 100);

    if (healRoll < (10 * _inventory.GetItemQuantity(Item.VampireFangs))) {
      _health.Heal(1);
      return true;
    }

    return false;
  }

  protected virtual bool DamageEnemy(EnemyHealth enemyHealth, float damage) {
    List<StatusCondition> statuses = new List<StatusCondition>();

    foreach (StatusCondition modifier in _weaponModifiers) {
      int randomNum = UnityEngine.Random.Range(0, 100);
      
      if (randomNum < StatusConfig.StatusEffectChance && !statuses.Contains(modifier)) {
        statuses.Add(modifier);
      }
    }

    float proteinDamage = AddProteinDamage(damage);
    float baseDmgInclCrit = HandleCrits(proteinDamage, statuses, enemyHealth);
    float coinBonus = AddCoinDamage();
    float rageBonus = AddRageDamage();

    float totalAmount = baseDmgInclCrit + coinBonus + rageBonus;

    if (statuses.Any()) {
      return enemyHealth.DamageWithStatusesAndType(totalAmount, statuses, _damageType);
    } else {
      return enemyHealth.DamageWithType(totalAmount, _damageType);
    }
  }

  // TODO: Incorporate protein damage
  protected virtual float AddProteinDamage(float originalAmount) {
    return originalAmount * (_inventory.HasItem(Item.ProteinPowder) ? 1.2f : 1.0f);
  }

  protected virtual float AddCoinDamage() {
    // Bonus of 0 to 7 damage max, capped at 30 coins
    if (_inventory.HasItem(Item.GoldChainNecklace)) {
      float bonusAmount = _wallet.GetBalance() * 0.233f;
      if (bonusAmount > 7) bonusAmount = 7;
      return bonusAmount;
    }

    return 0;
  }

  protected virtual float AddRageDamage() {
    // Bonus of 3 to 10 damage (50% HP to 0%)
    if (_health.BelowHalfHealth() && _inventory.HasItem(Item.VikingHelmet)) {
      float bonusAmount = 2;
      if (_health.CurrentHealthPercentage() < 0.4) bonusAmount += 1;
      if (_health.CurrentHealthPercentage() < 0.3) bonusAmount += 2;
      if (_health.CurrentHealthPercentage() < 0.2) bonusAmount += 2;
      if (_health.CurrentHealthPercentage() < 0.1) bonusAmount += 3;
      return bonusAmount;
    }

    return 0;
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

  protected virtual void PlaySound(AudioClip clip) {
    Instantiate(_singleTimeSound, transform.position, Quaternion.identity)
      .GetComponent<SingleTimeSound>()
      .LoadClipAndPlay(clip);
  }

  protected void ResetRotation() {
    Vector3 mousePosInWorld = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
    Vector2 vectorToMouse = VectorHelper.GetVectorToPoint(transform.position, mousePosInWorld);

    float angle = Vector2.SignedAngle(Vector2.up, vectorToMouse);
    transform.eulerAngles = new Vector3(0, 0, angle);
  }

  protected abstract void OnDrawGizmosSelected();

  public void SetSpeed(float speed) {
    _speed = speed;
    _anim.speed = _speed;
  }

  public virtual void MultiplyDamage(float modifier) {
    _damageAmount *= modifier;
  }
}
