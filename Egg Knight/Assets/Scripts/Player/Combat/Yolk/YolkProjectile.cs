using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class YolkProjectile : Projectile {
  private bool _isHoming = false;
  private Transform _nearestEnemy;

  [SerializeField] private GameObject _shardPrefab;
  [SerializeField] private int _numShardsOnDespawn;
  private bool _isShellShot = false;
  private YolkUpgradeManager upgrades;

  [SerializeField] private float _homingSpeed;

  protected override void Awake() {
    upgrades = GameObject.FindGameObjectWithTag("Player").GetComponent<YolkUpgradeManager>();

    _isHoming = upgrades.HasUpgrade(YolkUpgradeType.SmartYolk);
    if (_isHoming) {
      _nearestEnemy = GetNearestEnemy();
    }

    _isShellShot = upgrades.HasUpgrade(YolkUpgradeType.ShellShot);

    base.Awake();
  }

  private Transform GetNearestEnemy() {
    Transform nearestEnemy = null;

    float minDistance = float.MaxValue;

    GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("Enemy");

    foreach(GameObject enemy in allEnemies) {
      if (!(enemy.GetComponent<DeadTreeBehavior>() == null &
            enemy.GetComponent<CactusBehavior>() == null &
            enemy.GetComponent<EnemyBehaviour>().isDead)) continue;
      float currentDistance = Vector3.Distance(transform.position, enemy.transform.position);

      if (currentDistance < minDistance && enemy.layer == LayerMask.NameToLayer("Enemy")) {
        nearestEnemy = enemy.transform;
        minDistance = currentDistance;
      }
    }

    return nearestEnemy;
  }

  protected override void Despawn() {
    if (_isShellShot) {
      SpawnShards();
    }

    Destroy(gameObject);
  }

  private void SpawnShards() {
    for (int i = 0; i < _numShardsOnDespawn; i++) {
      int randomAngle = Random.Range(0, 360);
      Vector2 direction = Quaternion.Euler(0, 0, randomAngle) * Vector2.up;

      GameObject shardObject = Instantiate(_shardPrefab, transform.position, Quaternion.identity);
      YolkShard shard = shardObject.GetComponent<YolkShard>();
      if (upgrades.HasUpgrade(YolkUpgradeType.RunnyYolk)) shard.SetDamageMultiplier(0.33f);
      shard.SetDirection(direction, 0);
    }
  }

  private void FixedUpdate() {
    if (_isHoming && _nearestEnemy != null) {
      Vector2 direction = VectorHelper.GetVectorToPoint(transform.position, _nearestEnemy.position);

      float rotateAmount = Vector3.Cross(direction, transform.up).z;

      _rb.angularVelocity = -rotateAmount * _homingSpeed;
      _rb.velocity = transform.up * _speed;
    }
  }

  protected override void OnTriggerEnter2D(Collider2D collider) {
    EnemyHealth enemyHealth = collider.gameObject.GetComponent<EnemyHealth>();
    
    if (enemyHealth != null && collider.gameObject.layer == LayerMask.NameToLayer("Enemy")) {
      enemyHealth.DamageWithStatuses(_damage, new List<StatusCondition>() { StatusCondition.Yolked });
    }

    if (collider.gameObject.layer == LayerMask.NameToLayer("Obstacle") || enemyHealth != null) {
      StopCoroutine(DespawnTimer());
      Despawn();
    }
  }

  public void MultiplyDamage(float multiplyAmount) {
    _damage *= multiplyAmount;
  }
}
