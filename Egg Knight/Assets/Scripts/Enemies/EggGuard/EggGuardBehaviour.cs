using System;
using System.Collections;
using UnityEngine;

public class EggGuardBehaviour : EnemyBehaviour {
  [SerializeField] private float _maxDistanceToAttack = 2.0f;
  [SerializeField] private float _attackCooldown = 1.5f;

  private EggGuardState _state = EggGuardState.Chasing;

  protected override void Awake() {
    EggGuardHealth eggGuardHealth = gameObject.GetComponent<EggGuardHealth>();
    eggGuardHealth.OnEggGuardStatusDamage += HandleStatusDamage;

    EnemyBehaviour enemyBehaviour = gameObject.GetComponent<EnemyBehaviour>();
    enemyBehaviour.OnElectrocuted += HandleElectrocuted;

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

  private void Update() {
    Vector3 playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
    float _currentDistance = Vector3.Distance(transform.position, playerPos);

    if ((_currentDistance < _maxDistanceToAttack) && _state == EggGuardState.Chasing) {
      StartCoroutine(AttackPlayer());
    }
  }

  private void FixedUpdate() {
    if (_state == EggGuardState.Chasing) {
      Vector3 playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
      Vector2 vectorToPlayer = VectorHelper.GetVectorToPoint(transform.position, playerPos);

      _rb.velocity = vectorToPlayer * _currentSpeed;
    }
  }

  IEnumerator AttackPlayer() {
    _state = EggGuardState.Attacking;

    _rb.velocity = Vector2.zero;

    yield return new WaitForSeconds(_attackCooldown);

    _state = EggGuardState.Chasing;
  }
}
