using System;
using System.Collections;
using Pathfinding;
using UnityEngine;

public class EggGuardBehaviour : EnemyBehaviour {
  // [SerializeField] private float _maxDistanceToAttack = 2.0f;
  // [SerializeField] private float _attackCooldown = 1.5f;

  private EggGuardState _state = EggGuardState.Chasing;

  protected override void Awake() {
    EggGuardHealth eggGuardHealth = gameObject.GetComponent<EggGuardHealth>();
    eggGuardHealth.OnEggGuardStatusDamage += HandleStatusDamage;

    EnemyBehaviour enemyBehaviour = gameObject.GetComponent<EnemyBehaviour>();
    enemyBehaviour.OnElectrocuted += HandleElectrocuted;

    maxDistanceToAttack = 2.0f;
    attackCooldownMax = 1.5f;

    Health = eggGuardHealth;
    base.Awake();
  }

  private void HandleElectrocuted(object sender, EventArgs e) {
    StartCoroutine(Electrocute());
  }

  private IEnumerator Electrocute() {
    _state = EggGuardState.Stunned;
    _rb.velocity = Vector2.zero;

    yield return new WaitForSeconds(StatusConfig.ElectrocuteStunDuration);

    _state = EggGuardState.Chasing;
  }

  // private void Update() {
  //   Vector3 playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
  //   float _currentDistance = Vector3.Distance(transform.position, playerPos);
  //
  //   if ((_currentDistance < maxDistanceToAttack) && _state == EggGuardState.Chasing) {
  //     StartCoroutine(AttackPlayer());
  //   }
  // }
  //
  // private void FixedUpdate() {
  //   if (_state == EggGuardState.Chasing) {
  //     MoveToPlayer();
  //   }
  // }
  //
  protected override IEnumerator AttackPlayer() {
    isAttackOffCooldown = false;
  
    _rb.velocity = Vector2.zero;
  
    yield return new WaitForSeconds(attackCooldownMax);

    isAttackOffCooldown = true;
  }
}
