using System;
using System.Collections;
using UnityEngine;

public class GreaseOrb : Projectile {
  [SerializeField] private GameObject _greaseParticles;

  [SerializeField] private GameObject _echoObject;
  [SerializeField] private float _timeBetweenEcho;

  [SerializeField] private AudioClip _clip;
  [SerializeField] private GameObject _singleTimeSound;

  private Transform _playerTransform;

  protected override void Awake() {
    _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

    BaconHealth.OnBaconDeath += HandleBaconDeath;

    StartCoroutine(SpawnEcho());

    Instantiate(_singleTimeSound, transform.position, Quaternion.identity)
      .GetComponent<SingleTimeSound>()
      .LoadClipAndPlay(_clip);

    base.Awake();
  }

  private void HandleBaconDeath(object sender, EventArgs e) {
    StopCoroutine(DespawnTimer());
    Despawn();
  }

  protected override void Despawn() {
    Instantiate(_greaseParticles, transform.position, Quaternion.identity);

    Destroy(gameObject);
  }

  private void FixedUpdate() {
    Vector2 direction = VectorHelper.GetVectorToPoint(transform.position, _playerTransform.position);

    float rotateAmount = Vector3.Cross(direction, transform.up).z;

    _rb.angularVelocity = -rotateAmount * 200f;
    _rb.velocity = transform.up * _speed;
  }

  protected override void OnTriggerEnter2D(Collider2D collider) {
    PlayerHealth playerHealth = collider.gameObject.GetComponent<PlayerHealth>();
    
    if (playerHealth != null) {
      playerHealth.Damage(_damage);

      StopCoroutine(DespawnTimer());
      Despawn();
    }
  }

  private IEnumerator SpawnEcho() {
    while (true) {
      yield return new WaitForSeconds(_timeBetweenEcho);
      Instantiate(_echoObject, transform.position, Quaternion.identity);
    }
  }

  private void OnDestroy() {
    BaconHealth.OnBaconDeath -= HandleBaconDeath;
  }
}
