using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spoon : BasePlayerWeapon {
  [SerializeField] private Transform _attackPoint;
  [SerializeField] private float _attackWidth;
  [SerializeField] private float _attackHeight;

  [SerializeField] private LayerMask _obstacleLayer;

  private List<Collider2D> _hitEnemies;

  protected override void Awake() {
    _hitEnemies = new List<Collider2D>();
    
    base.Awake();
  }

  public override void EnableHitbox() {
    float hitboxAngle = transform.eulerAngles.z;

    Collider2D[] enemiesInRange = Physics2D.OverlapBoxAll(
      _attackPoint.position,
      new Vector2(_attackWidth, _attackHeight),
      hitboxAngle,
      _enemyLayer
    );
    
    Collider2D[] obstaclesInRange = Physics2D.OverlapBoxAll(
      _attackPoint.position,
      new Vector2(_attackWidth, _attackHeight),
      hitboxAngle,
      _obstacleLayer
    );

    Collider2D[] coinsInRange = Physics2D.OverlapBoxAll(
      _attackPoint.position,
      new Vector2(_attackWidth, _attackHeight),
      hitboxAngle,
      _coinLayer
    );

    List<Collider2D> notHitEnemies = new List<Collider2D>();

    foreach (Collider2D enemy in enemiesInRange) {
      if (_hitEnemies.Contains(enemy) == false) {
        _hitEnemies.Add(enemy);
        notHitEnemies.Add(enemy);
      }
    }
    
    foreach (Collider2D enemy in obstaclesInRange) {
      if (_hitEnemies.Contains(enemy) == false) {
        _hitEnemies.Add(enemy);
        notHitEnemies.Add(enemy);
      }
    }

    DamageEnemies(notHitEnemies.ToArray());
    CollectCoins(coinsInRange);
  }

  protected override void OnDrawGizmosSelected() {
    Gizmos.color = Color.red;
    Gizmos.DrawWireCube(_attackPoint.position, new Vector3(_attackWidth, _attackHeight, 1));
  }

  public override void OnAnimationEnd() {
    StartCoroutine(Despawn());
  }

  private IEnumerator Despawn() {
    yield return new WaitForSeconds(0.25f);

    base.OnAnimationEnd();
  }
}
