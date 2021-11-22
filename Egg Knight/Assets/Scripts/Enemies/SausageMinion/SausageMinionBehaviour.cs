using System;
using System.Collections;
using UnityEngine;

public class SausageMinionBehaviour : EnemyBehaviour {
  [SerializeField] private GameObject _confettiObject;

  protected override void Awake() {
    SausageMinionHealth sausageMinionHealth = GetComponent<SausageMinionHealth>();
    sausageMinionHealth.OnSausageMinionStatusDamage += HandleStatusDamage;

    EnemyBehaviour enemyBehaviour = GetComponent<EnemyBehaviour>();
    enemyBehaviour.OnElectrocuted += HandleElectrocuted;

    if (_confettiObject) {
      Instantiate(_confettiObject, transform.position, Quaternion.identity);
    }

    Health = sausageMinionHealth;
    base.Awake();
  }

  private void FixedUpdate() {
    if (isStunned || isDead) {
      rb.velocity = Vector2.zero;
    } else {
      rb.velocity = GetVectorToPlayer() * _currentSpeed;
    }
  }

  protected override void OnCollisionEnter2D(Collision2D other) {
    if (other.collider.gameObject.name == "SheriffSausage") {
      Physics2D.IgnoreCollision(other.collider, GetComponent<Collider2D>());
    }

    base.OnCollisionEnter2D(other);
  }

  private void HandleElectrocuted(object sender, EventArgs e) {
    StartCoroutine(Electrocute());
  }

  protected override IEnumerator Electrocute() {
    yield break;
  }

  protected override IEnumerator AttackPlayer() {
    yield break;
  }

  private Vector2 GetVectorToPlayer() {
    return VectorHelper.GetVectorToPoint(transform.position, _playerTransform.position);
  }
}
