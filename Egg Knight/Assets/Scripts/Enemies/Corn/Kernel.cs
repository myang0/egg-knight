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

  private float _moveSpeed;
  private float _rotationSpeed;

  private Vector3 _targetPoint;

  [SerializeField] private GameObject _singleTimeSound;
  [SerializeField] private AudioClip _beepClip;
  [SerializeField] private AudioClip _popClip;

  private void Awake() {
    _rb = GetComponent<Rigidbody2D>();
    _collider = GetComponent<Collider2D>();
    _sr = GetComponent<SpriteRenderer>();

    _origR = _sr.color.r;
    _origG = _sr.color.g;
    _origB = _sr.color.b;

    Vector3 playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
    _targetPoint = new Vector3(
      playerPos.x + Random.Range(-5f, 5f),
      playerPos.y + Random.Range(-5f, 5f),
      playerPos.z
    );

    _moveSpeed = Random.Range(0.2f, 0.4f);
    _rotationSpeed = Random.Range(-5f, 5f);
  }

  private void FixedUpdate() {
    if (_isAirborne) {
      transform.Rotate(0, 0, _rotationSpeed);

      transform.position = Vector3.MoveTowards(transform.position, _targetPoint, _moveSpeed);
      if (Vector3.Distance(transform.position, _targetPoint) < 0.01f) {
        StartCoroutine(ArmTimer());
      }
    }
  }

  private void OnTriggerEnter2D(Collider2D col) {
    if (_isArmed && col.gameObject.CompareTag("Player")) {
      PlayerHealth pHealth = col.GetComponent<PlayerHealth>();
      pHealth?.Damage(_damage);

      Instantiate(_singleTimeSound, transform.position, Quaternion.identity)
        .GetComponent<SingleTimeSound>()
        .LoadClipAndPlay(_popClip);

      StopCoroutine(ArmedColour());

      _isArmed = false;

      Disarm();
    }

    if (col.gameObject.layer == LayerMask.NameToLayer("Obstacle")) {
      Destroy(gameObject);
    }
  }

  private void Arm() {
    _isArmed = true;

    StartCoroutine(ArmedColour());
    StartCoroutine(DespawnTimer());
  }

  public void Disarm() {
    _sr.sprite = _popcornSprite;

    StopCoroutine(ArmedColour());
    StopCoroutine(DespawnTimer());
    StartCoroutine(Despawn(0.1f));
  }

  private IEnumerator ArmTimer() {
    _isAirborne = false;

    yield return new WaitForSeconds(0.5f);

    Arm();
  }

  private IEnumerator ArmedColour() {
    while (_isArmed) {
      _sr.color = Color.red;

      SingleTimeSound sound = Instantiate(_singleTimeSound, transform.position, Quaternion.identity)
        .GetComponent<SingleTimeSound>();

      sound.ScaleVolume(0.5f);
      sound.LoadClipAndPlay(_beepClip);

      yield return new WaitForSeconds(0.1f);

      _sr.color = Color.white;

      yield return new WaitForSeconds(0.5f);
    }
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
