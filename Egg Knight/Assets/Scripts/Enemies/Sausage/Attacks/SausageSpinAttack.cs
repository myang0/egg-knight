using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SausageSpinAttack : MonoBehaviour {
  [SerializeField] private int _numBulletsPerShot1;
  private float _angleBetweenShots1;

  [SerializeField] private float _waitTimeBetweenShots1;
  [SerializeField] private int _totalShots1;

  [SerializeField] private int _numBulletsPerShot2;
  private float _angleBetweenShots2;

  [SerializeField] private float _waitTimeBetweenShots2;
  [SerializeField] private int _totalShots2;
  
  [SerializeField] private Transform _pivot;
  [SerializeField] private Transform _shootPoint;

  [SerializeField] private GameObject _bulletObject;

  private List<Vector2> _bulletVectors1;
  private List<Vector2> _bulletVectors2;

  private Animator _anim;
  private EnemyBehaviour _eBehaviour;
  private SoundPlayer _soundPlayer;

  [SerializeField] private AudioClip _shootClip;

  private void Awake() {
    _bulletVectors1 = new List<Vector2>(new Vector2[_numBulletsPerShot1]);
    _bulletVectors2 = new List<Vector2>(new Vector2[_numBulletsPerShot2]);

    _angleBetweenShots1 = 360f / _numBulletsPerShot1;
    _angleBetweenShots2 = 360f / _numBulletsPerShot2;

    for (int i = 0; i < _numBulletsPerShot1; i++) {
      _bulletVectors1[i] = Quaternion.Euler(0, 0, _angleBetweenShots1 * i) * Vector2.up;
    }

    for (int i = 0; i < _numBulletsPerShot2; i++) {
      _bulletVectors2[i] = Quaternion.Euler(0, 0, _angleBetweenShots2 * i) * Vector2.up;
    }

    _anim = GetComponent<Animator>();
    _eBehaviour = GetComponent<EnemyBehaviour>();
    _soundPlayer = GetComponent<SoundPlayer>();
  }

  public void StartAttack() {
    int randomRoll = Random.Range(0, 50);

    if (randomRoll < 50) {
      StartCoroutine(Spin1());
    } else {
      StartCoroutine(Spin2());
    }
  }

  private IEnumerator Spin1() {
    StartCoroutine(SpinSounds());

    for (int i = 0; i < _totalShots1; i++) {
      for (int j = 0; j < _bulletVectors1.Count; j++) {
        Vector2 direction = _bulletVectors1[j];

        _pivot.eulerAngles = new Vector3(0, 0, Vector2.SignedAngle(Vector2.up, direction));
        SpawnBullet(direction);

        _bulletVectors1[j] = Quaternion.Euler(0, 0, Random.Range(-90f, 90f)) * direction;
      }

      yield return new WaitForSeconds(_waitTimeBetweenShots1);
    }

    _anim.SetBool("IsSpinning", false);
  }

  private IEnumerator Spin2() {
    for (int i = 0; i < _totalShots2; i++) {
      for (int j = 0; j < _bulletVectors2.Count; j++) {
        Vector2 direction = _bulletVectors2[j];

        _pivot.eulerAngles = new Vector3(0, 0, Vector2.SignedAngle(Vector2.up, direction));
        SpawnBullet(direction);

        _bulletVectors2[j] = Quaternion.Euler(0, 0, _angleBetweenShots2 / 48f) * direction;
      }

      yield return new WaitForSeconds(_waitTimeBetweenShots2);
    }

    _anim.SetBool("IsSpinning", false);
  }

  private void SpawnBullet(Vector2 direction) {
    if (_eBehaviour.isDead) {
      return;
    }
    
    GameObject bulletObject = Instantiate(_bulletObject, _shootPoint.position, Quaternion.identity);
    SausageBullet bullet = bulletObject?.GetComponent<SausageBullet>();

    ProjectileHelper.Refrigerate(_eBehaviour.PlayerInventory, bullet);
    bullet.SetDirection(direction, Vector2.SignedAngle(Vector2.up, direction));
  }

  private IEnumerator SpinSounds() {
    while (_anim.GetBool("IsSpinning")) {
      yield return new WaitForSeconds(0.1f);

      _soundPlayer.PlayClip(_shootClip);
    }
  }
}
