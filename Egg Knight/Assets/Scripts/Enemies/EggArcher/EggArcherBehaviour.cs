using System;
using System.Collections;
using UnityEngine;

public class EggArcherBehaviour : EnemyBehaviour {
  private Animator _anim;
  private Collider2D _collider;

  protected override void Awake() {
    _anim = GetComponent<Animator>();
    _collider = GetComponent<Collider2D>();

    EggArcherHealth eggArcherHealth = gameObject.GetComponent<EggArcherHealth>();
    eggArcherHealth.OnEggArcherStatusDamage += HandleStatusDamage;

    EnemyBehaviour enemyBehaviour = gameObject.GetComponent<EnemyBehaviour>();
    enemyBehaviour.OnElectrocuted += HandleElectrocuted;

    maxDistanceToAttack = 5f;
    attackCooldownMax = 2.5f;

    Health = eggArcherHealth;
    base.Awake();
  }

  public bool HasClearShot() {
    Vector2 vectorToPlayer = VectorHelper.GetVectorToPoint(transform.position, _playerTransform.position);
    RaycastHit2D firstHit = Physics2D.Raycast(transform.position, vectorToPlayer);

    return firstHit.transform.gameObject.CompareTag("Player");
  }

  protected override void OnCollisionEnter2D(Collision2D other) {
    if (_anim.GetBool("IsRolling") && other.collider.gameObject.layer == LayerMask.NameToLayer("PlayerFootprint")) {
      Physics2D.IgnoreCollision(other.collider, _collider);
    }

    base.OnCollisionEnter2D(other);
  }

  private void HandleElectrocuted(object sender, EventArgs e) {
    StartCoroutine(Electrocute());
  }

  protected override IEnumerator AttackPlayer() {
    yield break;
  }
}
