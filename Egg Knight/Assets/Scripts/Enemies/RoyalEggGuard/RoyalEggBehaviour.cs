using System;
using System.Collections;
using UnityEngine;

public class RoyalEggBehaviour : EnemyBehaviour {
  [SerializeField] private float _minAlertTime;

  private bool _minAlertTimePassed = true;

  private RoyalEggAttack _reAttack;

  protected override void Awake() {
    _reAttack = GetComponent<RoyalEggAttack>();

    RoyalEggHealth royalEggHealth = gameObject.GetComponent<RoyalEggHealth>();
    royalEggHealth.OnRoyalEggStatusDamage += HandleStatusDamage;

    EnemyBehaviour enemyBehaviour = gameObject.GetComponent<EnemyBehaviour>();
    enemyBehaviour.OnElectrocuted += HandleElectrocuted;

    maxDistanceToAttack = 5f;
    attackCooldownMax = 2.5f;

    Health = royalEggHealth;
    base.Awake();
  }

  private void HandleElectrocuted(object sender, EventArgs e) {
    StartCoroutine(Electrocute());
  }

  protected override IEnumerator AttackPlayer() {
    isAttackOffCooldown = false;
    isInAttackAnimation = true;
  
    rb.velocity = Vector2.zero;

    _reAttack.StartAttack();
  
    yield return new WaitForSeconds(attackCooldownMax);

    isAttackOffCooldown = true;
    isInAttackAnimation = false;
  }

  public override bool GetIsAttackReady() {
    return GetDistanceToPlayer() < maxDistanceToAttack && isAttackOffCooldown && !isInAttackAnimation && _minAlertTimePassed;
  }

  public void StartMinAlertTime() {
    StartCoroutine(AlertTime());
  }

  private IEnumerator AlertTime() {
    _minAlertTimePassed = false;

    yield return new WaitForSeconds(_minAlertTime);

    _minAlertTimePassed = true;
  }
}
