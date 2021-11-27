using System;
using System.Collections;
using Pathfinding;
using Stage;
using UnityEngine;

public class EggGuardBehaviour : EnemyBehaviour {
  
  protected override void Awake() {
    EggGuardHealth eggGuardHealth = gameObject.GetComponent<EggGuardHealth>();
    eggGuardHealth.OnEggGuardStatusDamage += HandleStatusDamage;

    EnemyBehaviour enemyBehaviour = gameObject.GetComponent<EnemyBehaviour>();
    enemyBehaviour.OnElectrocuted += HandleElectrocuted;

    maxDistanceToAttack = 4f;
    attackCooldownMax = 1.5f;
    
    int level = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>().level;
    if (level > 1) {
      _maxSpeed += 1f;
      eggGuardHealth.AddToMaxHealth(10);
    }

    if (level > 2) {
      _maxSpeed += 1f;
      eggGuardHealth.AddToMaxHealth(10);
    }

    Health = eggGuardHealth;
    base.Awake();
  }

  private void HandleElectrocuted(object sender, EventArgs e) {
    StartCoroutine(Electrocute());
  }

  protected override IEnumerator AttackPlayer() {
    isAttackOffCooldown = false;
    isInAttackAnimation = true;
  
    rb.velocity = Vector2.zero;
  
    yield return new WaitForSeconds(attackCooldownMax);

    isAttackOffCooldown = true;
    isInAttackAnimation = false;
  }
}
