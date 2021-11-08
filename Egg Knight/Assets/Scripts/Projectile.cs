using System;
using System.Collections;
using UnityEngine;

public abstract class Projectile : MonoBehaviour {
  [SerializeField] protected float _lifetime;
  [SerializeField] protected float _damage;
  [SerializeField] protected float _speed;

  protected Rigidbody2D _rb;

  protected virtual void Awake() {
    StartCoroutine(DespawnTimer());

    _rb = gameObject.GetComponent<Rigidbody2D>();
  }

  protected virtual IEnumerator DespawnTimer() {
    yield return new WaitForSeconds(_lifetime);

    Despawn();
  }

  protected abstract void Despawn();

  protected abstract void OnTriggerEnter2D(Collider2D collider);

  public void MultiplySpeed(float multiplier) {
    _speed *= multiplier;
  }

  public void SetDirection(Vector2 direction, float angle) {
    _rb.velocity = direction * _speed;

    transform.eulerAngles = new Vector3(0, 0, angle);
  }
}
