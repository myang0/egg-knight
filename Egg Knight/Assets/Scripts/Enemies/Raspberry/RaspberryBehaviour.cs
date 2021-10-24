using System;
using System.Collections;
using Stage;
using UnityEngine;

public class RaspberryBehaviour : EnemyBehaviour {
  [SerializeField] private GameObject _projectilePrefab;
  
  private int _shotsPerAttack;
  private int _maxShotsPerAttack = 3;
  private float _delayBetweenShots = 0.5f;

  protected override void Awake() {
    RaspberryHealth raspberryHealth = gameObject.GetComponent<RaspberryHealth>();
    raspberryHealth.OnRaspberryStatusDamage += HandleStatusDamage;

    EnemyBehaviour enemyBehaviour = gameObject.GetComponent<EnemyBehaviour>();
    enemyBehaviour.OnElectrocuted += HandleElectrocuted;

    attackCooldownMax = 5;
    maxDistanceToAttack = 8;
    minDistanceToAttack = 5;

    Health = raspberryHealth;
    IsWallCollisionOn = true;
    base.Awake();
  }

  private void HandleElectrocuted(object sender, EventArgs e) {
    StartCoroutine(Electrocute());
  }

  public override void Attack() {
    StartCoroutine(AttackPlayer());
    StartCoroutine(AttackCooldown());
  }

  private IEnumerator AttackCooldown() {
    isAttackOffCooldown = false;
    yield return new WaitForSeconds(attackCooldownMax);
    isAttackOffCooldown = true;
  }

  protected override IEnumerator AttackPlayer() {
    _rb.velocity = Vector2.zero;

    isInAttackAnimation = true;
    _shotsPerAttack = _maxShotsPerAttack;
    while (_shotsPerAttack > 0) {
      if (isStunned) {
        _shotsPerAttack = 0;
        break;
      }
      
      yield return new WaitForSeconds(_delayBetweenShots);
      
      Vector3 playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
      Vector2 vectorToPlayer = VectorHelper.GetVectorToPoint(transform.position, playerPos);
      
      GameObject projectileObject = Instantiate(_projectilePrefab, transform.position, Quaternion.identity);
      RaspberryProjectile projectile = projectileObject.GetComponent<RaspberryProjectile>();
      projectile.SetDirection(vectorToPlayer, 0);

      _shotsPerAttack--;
      yield return null;
    }

    isInAttackAnimation = false;
  }
}
