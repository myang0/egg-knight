using System;
using System.Collections;
using Pathfinding;
using UnityEngine;

public class EggGuardBehaviour : EnemyBehaviour {
  
  protected override void Awake() {
    EggGuardHealth eggGuardHealth = gameObject.GetComponent<EggGuardHealth>();
    eggGuardHealth.OnEggGuardStatusDamage += HandleStatusDamage;

    EnemyBehaviour enemyBehaviour = gameObject.GetComponent<EnemyBehaviour>();
    enemyBehaviour.OnElectrocuted += HandleElectrocuted;

    maxDistanceToAttack = 4f;
    attackCooldownMax = 1.5f;

    Health = eggGuardHealth;
    base.Awake();
  }

  private void HandleElectrocuted(object sender, EventArgs e) {
    StartCoroutine(Electrocute());
  }

  protected override IEnumerator AttackPlayer() {
    isAttackOffCooldown = false;
    isInAttackAnimation = true;
  
    _rb.velocity = Vector2.zero;
  
    yield return new WaitForSeconds(attackCooldownMax);

    isAttackOffCooldown = true;
    isInAttackAnimation = false;
  }
}
