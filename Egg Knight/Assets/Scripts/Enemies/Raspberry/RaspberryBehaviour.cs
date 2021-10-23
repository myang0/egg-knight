using System;
using System.Collections;
using Stage;
using UnityEngine;

public class RaspberryBehaviour : EnemyBehaviour {
  [SerializeField] private GameObject _projectilePrefab;

  // [SerializeField] private float _attackCooldown;
  // private bool _attackOffCooldown = true;

  private RaspberryState _state = RaspberryState.Fleeing;
  private int _shotsPerAttack;
  private int _maxShotsPerAttack = 3;

  protected override void Awake() {
    RaspberryHealth raspberryHealth = gameObject.GetComponent<RaspberryHealth>();
    raspberryHealth.OnRaspberryStatusDamage += HandleStatusDamage;

    EnemyBehaviour enemyBehaviour = gameObject.GetComponent<EnemyBehaviour>();
    enemyBehaviour.OnElectrocuted += HandleElectrocuted;

    attackCooldownMax = 1;
    maxDistanceToAttack = 8;
    minDistanceToAttack = 2;

    Health = raspberryHealth;
    isWallCollisionOn = true;
    base.Awake();
  }

  private void HandleElectrocuted(object sender, EventArgs e) {
    StartCoroutine(Electrocute());
  }

  private IEnumerator Electrocute() {
    _state = RaspberryState.Stunned;

    _rb.velocity = Vector2.zero;

    yield return new WaitForSeconds(StatusConfig.ElectrocuteStunDuration);

    _state = RaspberryState.Fleeing;
  }

  private void Update() {
    // Vector3 playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
    // // float _currentDistance = Vector3.Distance(transform.position, playerPos);
    //
    // if ((GetDistanceToPlayer() >= _minDistanceToAttack) && _state == RaspberryState.Fleeing && isAttackOffCooldown) {
    //   _state = RaspberryState.Attacking;
    //   StartCoroutine(AttackPlayer());
    // }
    //
    // else if ((GetDistanceToPlayer() < _minDistanceToAttack) && _state == RaspberryState.Attacking) {
    //   _state = RaspberryState.Fleeing;
    // }
  }

  // private void FixedUpdate() {
  //   if (_state == RaspberryState.Fleeing) {
  //     Flee();
  //   }
  // }
  
  public override bool GetIsAttackReady() {
    return GetDistanceToPlayer() >= minDistanceToAttack && _state == RaspberryState.Fleeing && isAttackOffCooldown;
  }

  protected override IEnumerator AttackPlayer() {
    _rb.velocity = Vector2.zero;

    isAttackOffCooldown = false;
    _shotsPerAttack = _maxShotsPerAttack;
    while (_shotsPerAttack > 0) {
    // while (_state == RaspberryState.Attacking) {
    
      Vector3 playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
      Vector2 vectorToPlayer = VectorHelper.GetVectorToPoint(transform.position, playerPos);

      GameObject projectileObject = Instantiate(_projectilePrefab, transform.position, Quaternion.identity);
      RaspberryProjectile projectile = projectileObject.GetComponent<RaspberryProjectile>();
      projectile.SetDirection(vectorToPlayer, 0);

      yield return new WaitForSeconds(attackCooldownMax);

      isAttackOffCooldown = true;
    }
  }
}
