using System;
using System.Collections;
using System.Collections.Generic;
using Stage;
using UnityEngine;

public class StrawberryBehaviour : EnemyBehaviour {
  [SerializeField] private Transform _shootPoint;

  [SerializeField] private GameObject _projectilePrefab;

  private List<Vector2> _projectileVectors;

  protected override void Awake() {
    StrawberryHealth strawberryHealth = gameObject.GetComponent<StrawberryHealth>();
    strawberryHealth.OnStrawberryStatusDamage += HandleStatusDamage;

    EnemyBehaviour enemyBehaviour = gameObject.GetComponent<EnemyBehaviour>();
    enemyBehaviour.OnElectrocuted += HandleElectrocuted;

    maxDistanceToAttack = 5;
    
    int level = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>().level;
    if (level > 1) {
      strawberryHealth.AddToMaxHealth(10);
    }

    if (level > 2) {
      strawberryHealth.AddToMaxHealth(10);
    }

    Health = strawberryHealth;
    isWallCollisionOn = true;
    base.Awake();

    _projectileVectors = new List<Vector2> {
      Vector2.up,
      Vector2.right,
      Vector2.down,
      Vector2.left
    };
  }

  private void HandleElectrocuted(object sender, EventArgs e) {
    StartCoroutine(Electrocute());
  }

  protected override IEnumerator AttackPlayer() {
    rb.velocity = Vector2.zero;

    yield break;
  }

  public void ShootProjectiles() {
    for (int i = 0; i < _projectileVectors.Count; i++) {
      GameObject projectileObject = Instantiate(_projectilePrefab, _shootPoint.position, Quaternion.identity);
      StrawberryProjectile projectile = projectileObject?.GetComponent<StrawberryProjectile>();
      ProjectileHelper.Refrigerate(_playerInventory, projectile);

      Vector2 direction = _projectileVectors[i];
      projectile.SetDirection(direction, Vector2.SignedAngle(Vector2.up, direction));

      _projectileVectors[i] = Quaternion.Euler(0, 0, 45) * direction;
    }
  }
}
