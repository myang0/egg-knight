using System;
using System.Collections;
using UnityEngine;

public class EggGuardBehaviour : EnemyBehaviour {
  [SerializeField] private float _maxDistanceToAttack = 2.0f;
  [SerializeField] private float _attackCooldown = 1.5f;

  private bool _isAttacking = false;

  protected override void Awake() {
    EggGuardHealth eggGuardHealth = gameObject.GetComponent<EggGuardHealth>();
    eggGuardHealth.OnEggGuardStatusDamage += HandleStatusDamage;

    base.Awake();
  }

  private void Update() {
    Vector3 playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
    float _currentDistance = Vector3.Distance(transform.position, playerPos);

    if ((_currentDistance < _maxDistanceToAttack) && _isAttacking == false) {
      StartCoroutine(AttackPlayer());
    }
  }

  private void FixedUpdate() {
    if (_isAttacking == false) {
      Vector3 playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
      Vector2 vectorToPlayer = VectorHelper.GetVectorToPoint(transform.position, playerPos);

      _rb.velocity = vectorToPlayer * _currentSpeed;
    }
  }

  IEnumerator AttackPlayer() {
    _isAttacking = true;

    _rb.velocity = Vector2.zero;

    yield return new WaitForSeconds(_attackCooldown);

    _isAttacking = false;
  }
}
