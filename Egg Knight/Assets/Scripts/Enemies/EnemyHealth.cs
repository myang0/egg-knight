using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyHealth : Health {
  [SerializeField] protected DamageType _weakTo;
  [SerializeField] protected DamageType _immuneTo;
  [SerializeField] protected DamageType _immuneTo2;
  

  protected PlayerInventory _playerInventory;
  protected PlayerCursedInventory _cursedInventory;
  protected YolkUpgradeManager _yolkUpgrades;

  [SerializeField] protected GameObject _healingYolkPrefab;

  protected bool _isFrosted = false;

  protected override void Awake() {
    GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

    _playerInventory = playerObject.GetComponent<PlayerInventory>();
    _cursedInventory = playerObject.GetComponent<PlayerCursedInventory>();
    _yolkUpgrades = playerObject.GetComponent<YolkUpgradeManager>();

    EnemyBehaviour enemyBehaviour = gameObject.GetComponent<EnemyBehaviour>();

    enemyBehaviour.OnFrosted += HandleFrosted;
    enemyBehaviour.OnIgnited += HandleIgnited;
    enemyBehaviour.OnElectrocuted += HandleElectrocuted;
    enemyBehaviour.OnBleed += HandleBleed;
    enemyBehaviour.OnYolked += HandleYolked;
    enemyBehaviour.OnWeakened += HandleWeakened;

    if (_playerInventory != null && _playerInventory.HasItem(Item.Norovirus)) {
      _maxHealth = _maxHealth * 0.9f;
    } else {
      base.Awake();
    }
  }

  protected virtual void HandleFrosted(object sender, EventArgs e) {
    StartCoroutine(Frost());
  }

  protected virtual IEnumerator Frost() {
    yield return new WaitForSeconds(0.05f);

    _isFrosted = true;

    yield return new WaitForSeconds(StatusConfig.FrostDuration);

    _isFrosted = false;
  }

  protected virtual void HandleIgnited(object sender, EventArgs e) {
    StartCoroutine(Ignite());
  }

  protected virtual IEnumerator Ignite() {
    for (int i = 0; i < StatusConfig.IgniteTicks; i++) {
      yield return new WaitForSeconds(StatusConfig.IgniteTimeBetweenTicks);

      Damage(StatusConfig.IgniteDamage);
    }
  }

  protected virtual void HandleElectrocuted(object sender, EventArgs e) {
    Damage(StatusConfig.ElectrocuteDamage);
  }

  protected virtual void HandleBleed(object sender, EventArgs e) {
    StartCoroutine(Bleed());
  }

  protected virtual IEnumerator Bleed() {
    for (int i = 0; i < StatusConfig.BleedTicks; i++) {
      yield return new WaitForSeconds(StatusConfig.BleedTimeBetweenTicks);

      Damage(StatusConfig.BleedDamage);
    }
  }

  protected virtual void HandleYolked(object sender, EventArgs e) {
    if (_yolkUpgrades.HasUpgrade(YolkUpgradeType.Salmonella)) {
      StartCoroutine(Yolked());
    }
  }

  protected virtual IEnumerator Yolked() {
    YolkManager yolkManager = GameObject.FindGameObjectWithTag("Player")?.GetComponent<YolkManager>();

    for (int i = 0; i < StatusConfig.SalmonellaTicks; i++) {
      yield return new WaitForSeconds(StatusConfig.SalmonellaTimeBetweenTicks);

      Damage(StatusConfig.SalmonellaDamage * yolkManager.DamageScaling);
    }
  }

  protected virtual void HandleWeakened(object sender, EventArgs e) {
    if (_armourValue == _initialArmourValue) {
      StartCoroutine(Weakened());
    }
  }

  protected virtual IEnumerator Weakened() {
    yield return new WaitForSeconds(0.01f);

    _armourValue *= StatusConfig.WeakenedArmourModifier;

    yield return new WaitForSeconds(StatusConfig.WeakenedDuration);

    _armourValue /= StatusConfig.WeakenedArmourModifier;
  }

  public override void Damage(float amount) {
    base.Damage(amount + ((_isFrosted) ? amount * StatusConfig.FrostDamageMultiplier : 0));
  }

  public abstract void DamageWithStatuses(float amount, List<StatusCondition> statuses);

  public abstract bool DamageWithStatusesAndType(float amount, List<StatusCondition> statuses, DamageType type);

  public virtual bool DamageWithType(float amount, DamageType type) {
    if (_currentHealth <= 0) return false;
    
    float bonusDamage = 0;

    if (type == _weakTo) {
      _takingCriticalDamage = true;
      bonusDamage = (amount * 0.5f);
    }

    if (type == _immuneTo || type == _immuneTo2) {
      Damage(0);
      return false;
    }
    else {
      PlayTypeSound(type);

      Damage(amount + bonusDamage);
      return true;
    }
  }

  private void PlayTypeSound(DamageType type) {
    if (type == DamageType.Slash) {
      SoundManager.Instance.PlaySound(Sound.Slash);
    } else if (type == DamageType.Pierce) {
      SoundManager.Instance.PlaySound(Sound.Pierce, minPitch: 0.9f, maxPitch: 1.1f);
    } else if (type == DamageType.Blunt) {
      SoundManager.Instance.PlaySound(Sound.Blunt, volumeScaling: 0.5f);
    } else {

    }
  }

  public bool GetIsHealthDamaged() {
    return _currentHealth < _maxHealth;
  }

  protected override void Die() {
    int healRoll = UnityEngine.Random.Range(0, 100);
    EnemyBehaviour enemyBehaviour = gameObject.GetComponent<EnemyBehaviour>();
    
    if (healRoll < (10f * _playerInventory.GetItemQuantity(Item.ChefHat)) && 
        !enemyBehaviour.disableRegularDrops &&
        !enemyBehaviour.notAffectedByDropMods) {
      Instantiate(_healingYolkPrefab, transform.position, Quaternion.identity);
    }

    base.Die();
  }
}
