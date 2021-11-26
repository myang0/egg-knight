using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class SausageWalkAttack : MonoBehaviour {
  [SerializeField] private float _walkMoveSpeed;

  [SerializeField] private int _numBursts;
  [SerializeField] private float _timeBetweenBursts;

  [SerializeField] private float _numShots;
  [SerializeField] private float _timeBetweenShots;

  [SerializeField] private GameObject _bulletObject;
  [SerializeField] private Transform _shootPoint;

  private Animator _anim;
  private Rigidbody2D _rb;
  private EnemyBehaviour _eBehaviour;

  private Transform _playerTransform;

  public static event EventHandler OnAttackStart;
  public static event EventHandler OnRevolverShot;
  public static event EventHandler OnAttackEnd;

  private void Awake() {
    _anim = GetComponent<Animator>();
    _rb = GetComponent<Rigidbody2D>();
    _eBehaviour = GetComponent<EnemyBehaviour>();

    _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
  }

  private void FixedUpdate() {
    if (_anim.GetBool("IsWalking")) {
      _rb.velocity = VectorToPlayer(transform) * _walkMoveSpeed;
    }
  }

  public void StartAttack() {
    OnAttackStart?.Invoke(this, EventArgs.Empty);

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
    OnAttackEnd?.Invoke(this, EventArgs.Empty);
  }

  private void ShootAtPlayer() {
    Vector2 direction = VectorToPlayer(_shootPoint);
    Vector2 directionWithRecoil = Quaternion.Euler(0, 0, Random.Range(-25f, 25f)) * direction;

    GameObject bulletObject = Instantiate(_bulletObject, _shootPoint.position, Quaternion.identity);
    SausageBullet bullet = bulletObject?.GetComponent<SausageBullet>();
    ProjectileHelper.Refrigerate(_eBehaviour.PlayerInventory, bullet);

    bullet.SetDirection(directionWithRecoil, Vector2.SignedAngle(Vector2.up, directionWithRecoil));

    OnRevolverShot?.Invoke(this, EventArgs.Empty);
  }

  private Vector2 VectorToPlayer(Transform originTransform) {
    return VectorHelper.GetVectorToPoint(originTransform.position, _playerTransform.position);
  }
}
