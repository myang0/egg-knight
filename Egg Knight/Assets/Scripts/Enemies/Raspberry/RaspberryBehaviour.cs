using System.Collections;
using Stage;
using UnityEngine;

public class RaspberryBehaviour : EnemyBehaviour {
  [SerializeField] private GameObject _projectilePrefab;

  [SerializeField] private float _minDistanceToAttack;

  [SerializeField] private float _attackCooldown;
  private bool _attackOffCooldown = true;

  private RaspberryState _state = RaspberryState.Fleeing;

  protected override void Awake() {
    RaspberryHealth raspberryHealth = gameObject.GetComponent<RaspberryHealth>();
    raspberryHealth.OnRaspberryStatusDamage += HandleStatusDamage;
    Health = raspberryHealth;
    base.Awake();
  }

  private void Update() {
    Vector3 playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
    float _currentDistance = Vector3.Distance(transform.position, playerPos);

    if ((_currentDistance >= _minDistanceToAttack) && _state == RaspberryState.Fleeing && _attackOffCooldown) {
      _state = RaspberryState.Attacking;
      StartCoroutine(AttackPlayer());
    }

    else if ((_currentDistance < _minDistanceToAttack) && _state == RaspberryState.Attacking) {
      _state = RaspberryState.Fleeing;
    }
  }

  private void FixedUpdate() {
    if (_state == RaspberryState.Fleeing) {
      Vector3 playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
      Vector2 vectorFromPlayer = VectorHelper.GetVectorToPoint(playerPos, transform.position);

      _rb.velocity = vectorFromPlayer * _currentSpeed;
    }
  }

  private IEnumerator AttackPlayer() {
    _rb.velocity = Vector2.zero;

    while (_state == RaspberryState.Attacking) {
      _attackOffCooldown = false;

      Vector3 playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
      Vector2 vectorToPlayer = VectorHelper.GetVectorToPoint(transform.position, playerPos);

      GameObject projectileObject = Instantiate(_projectilePrefab, transform.position, Quaternion.identity);
      RaspberryProjectile projectile = projectileObject.GetComponent<RaspberryProjectile>();
      projectile.SetDirection(vectorToPlayer, 0);

      yield return new WaitForSeconds(_attackCooldown);

      _attackOffCooldown = true;
    }
  }
}
