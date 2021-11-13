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

  [SerializeField] private GameObject _bulletObject;

  private List<Vector2> _bulletVectors1;
  private List<Vector2> _bulletVectors2;

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
  }

  public void StartAttack() {
    int randomRoll = Random.Range(0, 100);

    if (randomRoll < 50) {
      StartCoroutine(Spin1());
    } else {
      StartCoroutine(Spin2());
    }
  }

  private IEnumerator Spin1() {
    for (int i = 0; i < _totalShots1; i++) {
      for (int j = 0; j < _bulletVectors1.Count; j++) {
        Vector2 direction = _bulletVectors1[j];

        SpawnBullet(direction);

        _bulletVectors1[j] = Quaternion.Euler(0, 0, _angleBetweenShots1 / 4f) * direction;
      }

      yield return new WaitForSeconds(_waitTimeBetweenShots1);
    }
  }

  private IEnumerator Spin2() {
    for (int i = 0; i < _totalShots2; i++) {
      for (int j = 0; j < _bulletVectors2.Count; j++) {
        Vector2 direction = _bulletVectors2[j];

        SpawnBullet(direction);

        _bulletVectors2[j] = Quaternion.Euler(0, 0, _angleBetweenShots2 / 16f) * direction;
      }

      yield return new WaitForSeconds(_waitTimeBetweenShots2);
    }
  }

  private void SpawnBullet(Vector2 direction) {
    GameObject bulletObject = Instantiate(_bulletObject, transform.position, Quaternion.identity);
    SausageBullet bullet = bulletObject?.GetComponent<SausageBullet>();

    bullet.SetDirection(direction, Vector2.SignedAngle(Vector2.up, direction));
  }
}
