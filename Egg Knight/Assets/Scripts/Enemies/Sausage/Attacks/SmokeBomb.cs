using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class SmokeBomb : MonoBehaviour {
  private Rigidbody2D _rb;

  [SerializeField] private GameObject _smokeParticlesObject;

  [SerializeField] private LayerMask _playerLayer;

  [SerializeField] private float _explosionRange;
  [SerializeField] private float _explosionDamage;

  private float _rotationSpeed;
  private float _targetY;

  private void Awake() {
    _rb = GetComponent<Rigidbody2D>();

    Vector2 forceVector = Quaternion.Euler(0, 0, Random.Range(-10f, 10f)) * Vector2.up;
    _rb.AddForce(forceVector * Random.Range(15f, 20f), ForceMode2D.Impulse);

    _rotationSpeed = Random.Range(-10f, 10f);

    _targetY = transform.position.y + Random.Range(-3f, 3f);

    SausageHealth.OnSausageDeath += HandleBossDeath;
  }

  private void HandleBossDeath(object sender, EventArgs e) {
    Destroy(gameObject);
  }

  private void FixedUpdate() {
    transform.Rotate(0, 0, _rotationSpeed);

    if (transform.position.y <= _targetY && _rb.velocity.y < 0) {
      Explode();
    }
  }

  private void Explode() {
    Collider2D[] entitiesInRange = Physics2D.OverlapCircleAll(transform.position, _explosionRange, _playerLayer);

    foreach (Collider2D entity in entitiesInRange) {
      PlayerHealth pHealth = entity.GetComponent<PlayerHealth>();

      pHealth?.Damage(_explosionDamage);
    }

    Instantiate(_smokeParticlesObject, transform.position, Quaternion.identity);
    Destroy(gameObject);
  }

  private void OnDrawGizmosSelected() {
    Gizmos.color = Color.red;
    Gizmos.DrawWireSphere(transform.position, _explosionRange);
  }

  private void OnDestroy() {
    SausageHealth.OnSausageDeath -= HandleBossDeath;
  }
}
