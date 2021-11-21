using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class SausageMinionGun : MonoBehaviour {
  [SerializeField] private SausageMinionHealth _smHealth;
  private bool _isAlive = true;

  [SerializeField] private GameObject _bulletObject;
  [SerializeField] private Transform _shootPoint;

  [SerializeField] private float _minTimeBetweenShots;
  [SerializeField] private float _maxTimeBetweenShots;

  private float _timeBetweenShots;

  private Transform _playerTransform;

  [SerializeField] private MinionRevolverSprite _revolverSprite;

  private void Awake() {
    _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

    _timeBetweenShots = Random.Range(_minTimeBetweenShots, _maxTimeBetweenShots);

    if (_smHealth != null) {
      _smHealth.OnSausageMinionDeath += HandleDeath;
    }

    StartCoroutine(Shoot());
  }

  void HandleDeath(object sender, EventArgs e) {
    _isAlive = false;
  }

  private void FixedUpdate() {
    float angleToPlayer = Vector2.SignedAngle(Vector2.up, GetVectorToPlayer());

    transform.eulerAngles = new Vector3(0, 0, angleToPlayer);
  }

  private IEnumerator Shoot() {
    while(_isAlive) {
      yield return new WaitForSeconds(_timeBetweenShots);

      if (_isAlive) {
        GameObject bulletObject = Instantiate(_bulletObject, _shootPoint.position, Quaternion.identity);
        SausageBullet bullet = bulletObject.GetComponent<SausageBullet>();

        bullet.SetDirection(GetVectorToPlayer(), transform.eulerAngles.z);

        if (_revolverSprite != null) {
          _revolverSprite.PlayAnimation();
        }
      }
    }
  }

  private Vector2 GetVectorToPlayer() {
    return VectorHelper.GetVectorToPoint(transform.position, _playerTransform.position);
  }
}
