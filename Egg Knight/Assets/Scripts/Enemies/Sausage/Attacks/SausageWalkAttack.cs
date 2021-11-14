using System.Collections;
using UnityEngine;

public class SausageWalkAttack : MonoBehaviour {
  [SerializeField] private float _walkMoveSpeed;

  [SerializeField] private int _numBursts;
  [SerializeField] private float _timeBetweenBursts;

  [SerializeField] private float _numShots;
  [SerializeField] private float _timeBetweenShots;

  [SerializeField] private GameObject _bulletObject;

  private Animator _anim;
  private Rigidbody2D _rb;

  private Transform _playerTransform;

  private void Awake() {
    _anim = GetComponent<Animator>();
    _rb = GetComponent<Rigidbody2D>();

    _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
  }

  private void FixedUpdate() {
    if (_anim.GetBool("IsWalking")) {
      _rb.velocity = VectorToPlayer() * _walkMoveSpeed;
    }
  }

  public void StartAttack() {
    StartCoroutine(Walk());
  }

  private IEnumerator Walk() {
    for (int i = 0; i < _numBursts; i++) {
      yield return new WaitForSeconds(_timeBetweenBursts);

      for (int j = 0; j < _numShots; j++) {
        ShootAtPlayer();
        yield return new WaitForSeconds(_timeBetweenShots);
      }
    }

    _anim.SetBool("IsWalking", false);
  }

  private void ShootAtPlayer() {
    Vector2 direction = VectorToPlayer();
    Vector2 directionWithRecoil = Quaternion.Euler(0, 0, Random.Range(-25f, 25f)) * direction;

    GameObject bulletObject = Instantiate(_bulletObject, transform.position, Quaternion.identity);
    SausageBullet bullet = bulletObject?.GetComponent<SausageBullet>();

    bullet.SetDirection(directionWithRecoil, Vector2.SignedAngle(Vector2.up, directionWithRecoil));
  }

  private Vector2 VectorToPlayer() {
    return VectorHelper.GetVectorToPoint(transform.position, _playerTransform.position);
  }
}
