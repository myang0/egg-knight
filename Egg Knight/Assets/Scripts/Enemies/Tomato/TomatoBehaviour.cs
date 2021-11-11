using System;
using System.Collections;
using Pathfinding;
using UnityEngine;

public class TomatoBehaviour : EnemyBehaviour {
  private Collider2D _collider;

  private TomatoHealth _tHealth;

  [SerializeField] private GameObject _explosionObject;

  protected override void Awake() {
    _collider = GetComponent<Collider2D>();

    _tHealth = GetComponent<TomatoHealth>();
    _tHealth.OnTomatoStatusDamage += HandleStatusDamage;

    EnemyBehaviour enemyBehaviour = GetComponent<EnemyBehaviour>();
    enemyBehaviour.OnElectrocuted += HandleElectrocuted;

    maxDistanceToAttack = 4f;

    Health = _tHealth;
    base.Awake();
  }

  private void HandleElectrocuted(object sender, EventArgs e) {
    StartCoroutine(Electrocute());
  }

  protected override IEnumerator AttackPlayer() {
    _collider.isTrigger = true;

    isInAttackAnimation = true;

    Vector3 playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
    Vector2 vectorToPlayer = VectorHelper.GetVectorToPoint(transform.position, playerPos);

    float distanceToPlayer = Vector2.Distance(transform.position, playerPos);
    rb.velocity = vectorToPlayer * (distanceToPlayer * 1.5f);

    yield return new WaitForSeconds(0.5f);

    if (_tHealth.CurrentHealth > 0) {
      Explode();
    }
  }

  public void Explode() {
    Instantiate(_explosionObject, transform.position, Quaternion.identity);
    _tHealth.OnExplode();
  }
}
