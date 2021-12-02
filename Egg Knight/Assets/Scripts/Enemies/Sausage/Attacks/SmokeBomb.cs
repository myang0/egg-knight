using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class SmokeBomb : MonoBehaviour {  
  private Rigidbody2D _rb;

  [SerializeField] private GameObject _crosshairObject;
  private Crosshair _currentCrosshair;

  [SerializeField] private GameObject _smokeParticlesObject;

  [SerializeField] private LayerMask _playerLayer;

  [SerializeField] private float _explosionRange;
  [SerializeField] private float _explosionDamage;

  [SerializeField] private GameObject _singleTimeSound;
  [SerializeField] private AudioClip _explodeClip;

  private float _rotationSpeed;
  private float _targetY;

  private void Awake() {
    _rb = GetComponent<Rigidbody2D>();

    Vector2 forceVector = Quaternion.Euler(0, 0, Random.Range(-10f, 10f)) * Vector2.up;
    _rb.AddForce(forceVector * Random.Range(50f, 75f), ForceMode2D.Impulse);
    _rotationSpeed = Random.Range(-10f, 10f);

    _targetY = transform.position.y;

    SausageHealth.OnSausageDeath += HandleBossDeath;

    StartCoroutine(DropTimer());
  }

  private void HandleBossDeath(object sender, EventArgs e) {
    Destroy(gameObject);
  }

  private void FixedUpdate() {
    transform.Rotate(0, 0, _rotationSpeed);

    if (transform.position.y <= _targetY && _rb.velocity.y < 0) {
      Explode();

      if (_currentCrosshair != null) {
        _currentCrosshair.FadeOut();
      }
    }
  }

  private void Explode() {
    Collider2D[] entitiesInRange = Physics2D.OverlapCircleAll(transform.position, _explosionRange, _playerLayer);

    foreach (Collider2D entity in entitiesInRange) {
      PlayerHealth pHealth = entity.GetComponent<PlayerHealth>();

      pHealth?.Damage(_explosionDamage);
    }

    SingleTimeSound sound = Instantiate(_singleTimeSound, transform.position, Quaternion.identity)
      .GetComponent<SingleTimeSound>();

    sound.ScaleVolume(0.8f);
    sound.LoadClipAndPlay(_explodeClip);

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

  private IEnumerator DropTimer() {
    float waitTime = Random.Range(0.5f, 1f);
    yield return new WaitForSeconds(waitTime);

    Drop();
  }

  private void Drop() {
    _rb.velocity = Vector2.zero;

    Vector3 playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
    
    Vector3 target = new Vector3(playerPos.x + Random.Range(-4f, 4f), playerPos.y + Random.Range(-4f, 4f), playerPos.z);
    transform.position = new Vector3(target.x, target.y + 50, target.z);

    _targetY = target.y;

    _currentCrosshair = Instantiate(_crosshairObject, target, Quaternion.identity).GetComponent<Crosshair>();
  }
}
