using System;
using System.Collections;
using UnityEngine;

public class EggGuardBehaviour : MonoBehaviour {
  private Rigidbody2D _rb;

  [SerializeField] private float _speed = 3.0f;

  [SerializeField] private float _maxDistanceToAttack = 2.0f;
  [SerializeField] private float _attackCooldown = 1.5f;

  private bool _isAttacking = false;

  private void Awake() {
    _rb = gameObject.GetComponent<Rigidbody2D>();

    EggGuardHealth.OnEggGuardStatusDamage += HandleStatusDamage; 
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

      _rb.velocity = vectorToPlayer * _speed;
    }
  }

  IEnumerator AttackPlayer() {
    _isAttacking = true;

    _rb.velocity = Vector2.zero;

    yield return new WaitForSeconds(_attackCooldown);

    _isAttacking = false;
  }

  private void HandleStatusDamage(object sender, EnemyStatusEventArgs e) {
    if (e.status == StatusCondition.Yolked) {
      StartCoroutine(Yolked());
    }
  }

  IEnumerator Yolked() {
    _speed /= 2;

    yield return new WaitForSeconds(2);

    _speed *= 2;
  }
}
