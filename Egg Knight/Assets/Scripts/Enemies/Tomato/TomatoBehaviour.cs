using System;
using System.Collections;
using Pathfinding;
using UnityEngine;
using Random = UnityEngine.Random;

public class TomatoBehaviour : EnemyBehaviour {
  private Collider2D _collider;

  private TomatoHealth _tHealth;

  private Vector3 _targetPoint;

  [SerializeField] private GameObject _explosionObject;

  protected override void Awake() {
    _collider = GetComponent<Collider2D>();

    _tHealth = GetComponent<TomatoHealth>();
    _tHealth.OnTomatoStatusDamage += HandleStatusDamage;

    EnemyBehaviour enemyBehaviour = GetComponent<EnemyBehaviour>();
    enemyBehaviour.OnElectrocuted += HandleElectrocuted;

    maxDistanceToAttack = 5f;

    Health = _tHealth;
    base.Awake();
  }

  private void FixedUpdate() {
    if (isInAttackAnimation) {
      transform.position = Vector3.MoveTowards(transform.position, _targetPoint, 0.4f);
    }
  }

  private void HandleElectrocuted(object sender, EventArgs e) {
    StartCoroutine(Electrocute());
  }

  protected override IEnumerator AttackPlayer() {
    _collider.isTrigger = true;

    isInAttackAnimation = true;

    _targetPoint = new Vector3(
      _playerTransform.position.x + Random.Range(-1f, 1f),
      _playerTransform.position.y + Random.Range(-1f, 1f),
      _playerTransform.position.z
    );

    yield break;
  }

  public void Explode() {
    if (_tHealth.CurrentHealth > 0) {
      Instantiate(_explosionObject, transform.position, Quaternion.identity);
      _tHealth.OnExplode();
    }
  }
}
