using System;
using System.Collections;
using Stage;
using UnityEngine;

public class RaspberryBehaviour : EnemyBehaviour {
  [SerializeField] private GameObject _projectilePrefab;

  [SerializeField] private Transform _shootPoint;
  
  private int _shotsPerAttack;
  private int _maxShotsPerAttack = 2;

  protected override void Awake() {
    RaspberryHealth raspberryHealth = gameObject.GetComponent<RaspberryHealth>();
    raspberryHealth.OnRaspberryStatusDamage += HandleStatusDamage;

    EnemyBehaviour enemyBehaviour = gameObject.GetComponent<EnemyBehaviour>();
    enemyBehaviour.OnElectrocuted += HandleElectrocuted;

    attackCooldownMax = 5;
    maxDistanceToAttack = 8;
    minDistanceToAttack = 5;

    int level = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>().level;
    if (level > 1) {
      _maxShotsPerAttack += 1;
      attackCooldownMax -= 1;
      maxDistanceToAttack += 2;
      minDistanceToAttack -= 1;
      _maxSpeed += 0.5f;
      raspberryHealth.AddToMaxHealth(10);
    }

    if (level > 2) {
      _maxShotsPerAttack += 1;
      attackCooldownMax -= 1;
      maxDistanceToAttack += 1;
      minDistanceToAttack -= 1;
      _maxSpeed += 0.5f;
      raspberryHealth.AddToMaxHealth(10);
    }

    Health = raspberryHealth;
    isWallCollisionOn = true;
    base.Awake();
  }

  private void HandleElectrocuted(object sender, EventArgs e) {
    _shotsPerAttack = _maxShotsPerAttack;

    StartCoroutine(Electrocute());
  }

  public override void Attack() {
    StartCoroutine(AttackPlayer());
  }

  private IEnumerator AttackCooldown() {
    isAttackOffCooldown = false;
    yield return new WaitForSeconds(attackCooldownMax);
    isAttackOffCooldown = true;
  }

  protected override IEnumerator AttackPlayer() {
    rb.velocity = Vector2.zero;

    isInAttackAnimation = true;
    _shotsPerAttack = _maxShotsPerAttack;
    
    yield break;
  }

  public void ShootProjectile() {
    Vector3 playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
    Vector2 vectorToPlayer = VectorHelper.GetVectorToPoint(transform.position, playerPos);
    
    GameObject projectileObject = Instantiate(_projectilePrefab, _shootPoint.position, Quaternion.identity);
    RaspberryProjectile projectile = projectileObject.GetComponent<RaspberryProjectile>();

    ProjectileHelper.Refrigerate(_playerInventory, projectile);
    projectile.SetDirection(vectorToPlayer, 0);

    CountShots();
  }

  private void CountShots() {
    _shotsPerAttack--;

    if (_shotsPerAttack <= 0) {
      isInAttackAnimation = false;
      StartCoroutine(AttackCooldown());
    }
  }
}
