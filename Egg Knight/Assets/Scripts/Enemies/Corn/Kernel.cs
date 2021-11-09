using System.Collections;
using UnityEngine;

public class Kernel : MonoBehaviour {
  private Rigidbody2D _rb;
  private Collider2D _collider;
  private SpriteRenderer _sr;

  private float _origR;
  private float _origG;
  private float _origB;

  [SerializeField] private Sprite _popcornSprite;

  [SerializeField] private float _timeToDespawn;

  [SerializeField] private float _damage;

  private bool _isAirborne = true;
  private bool _isArmed = false;

  private float _rotationSpeed;

  private float _targetY;

  private void Awake() {
    _rb = GetComponent<Rigidbody2D>();
    _collider = GetComponent<Collider2D>();
    _sr = GetComponent<SpriteRenderer>();

    _origR = _sr.color.r;
    _origG = _sr.color.g;
    _origB = _sr.color.b;

    Vector2 forceVector = Quaternion.Euler(0, 0, Random.Range(-12.5f, 12.5f)) * Vector2.up;
    _rb.AddForce(forceVector * Random.Range(15f, 25f), ForceMode2D.Impulse);

    _rotationSpeed = Random.Range(-5f, 5f);

    _targetY = transform.position.y + Random.Range(-2.5f, 0.5f);
  }

  private void FixedUpdate() {
    if (_isAirborne) {
      transform.Rotate(0, 0, _rotationSpeed);
    }

    if (_isAirborne && transform.position.y <= _targetY && _rb.velocity.y < 0) {
      Arm();
    }
  }

  private void OnTriggerEnter2D(Collider2D col) {
    if (_isArmed && col.gameObject.CompareTag("Player")) {
      PlayerHealth pHealth = col.GetComponent<PlayerHealth>();
      pHealth?.Damage(_damage);

      Disarm();
    }
  }

  private void Arm() {
    _rb.velocity = Vector2.zero;
    _rb.gravityScale = 0;

    _collider.enabled = true;

    _isAirborne = false;
    _isArmed = true;

    StartCoroutine(DespawnTimer());
  }

  public void Disarm() {
    _sr.sprite = _popcornSprite;

    StopCoroutine(DespawnTimer());
    StartCoroutine(Despawn(0.1f));
  }

  private IEnumerator DespawnTimer() {
    yield return new WaitForSeconds(_timeToDespawn);

    StartCoroutine(Despawn(0.25f));
  }

  private IEnumerator Despawn(float alphaChange) {
    _collider.enabled = false;

    float a = _sr.color.a;

    while (a > 0) {
      yield return new WaitForSeconds(0.05f);

      a -= alphaChange;
      _sr.color = new Color(_origR, _origG, _origB, a);
    }

    Destroy(gameObject);
  }
}
