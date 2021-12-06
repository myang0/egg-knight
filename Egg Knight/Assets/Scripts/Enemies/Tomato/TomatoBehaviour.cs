using System;
using System.Collections;
using Pathfinding;
using Stage;
using UnityEngine;
using Random = UnityEngine.Random;

public class TomatoBehaviour : EnemyBehaviour {
  [SerializeField] private float _attackDamage;
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
    attackCooldownMax = 3f;
    
    int level = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>().level;
    if (level > 2) {
      _maxSpeed += 0.5f;
      attackCooldownMax -= 0.5f;
      _tHealth.AddToMaxHealth(25);
    }

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
    isAttackOffCooldown = false;
    isInAttackAnimation = true;

    _targetPoint = new Vector3(
      _playerTransform.position.x + Random.Range(-1f, 1f),
      _playerTransform.position.y + Random.Range(-1f, 1f),
      _playerTransform.position.z
    );

    yield return new WaitForSeconds(attackCooldownMax);
    isAttackOffCooldown = true;
  }

  public void Explode() {
    if (_tHealth.CurrentHealth > 0 && Vector2.Distance(transform.position, _playerTransform.position) < 2.5) {
      Instantiate(_explosionObject, transform.position, Quaternion.identity);
      _tHealth.OnExplode();
    }
  }
  
  private void OnTriggerEnter2D(Collider2D col) {
    if (col.gameObject.CompareTag("Player") && !isInAttackAnimation) {
      GameObject playerObject = col.gameObject;
      PlayerHealth playerHealth = playerObject?.GetComponent<PlayerHealth>();

      playerHealth?.Damage(_attackDamage);
    }
  }
}
