using System;
using System.Collections;
using UnityEngine;

public class SausageSnipeAttack : MonoBehaviour {
  [SerializeField] private float _timeBeforeShot;

  [SerializeField] private GameObject _bulletObject;
  [SerializeField] private GameObject _laserObject;

  [SerializeField] private Transform _shootPoint;

  private Animator _anim;

  private Transform _playerTransform;

  public static event EventHandler OnAttackStart;
  public static event EventHandler OnRifleShot;
  public static event EventHandler OnAttackEnd;

  private void Awake() {
    _anim = GetComponent<Animator>();

    _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
  }

  public void StartAttack() {
    OnAttackStart?.Invoke(this, EventArgs.Empty);

    _laserObject.SetActive(true);

    StartCoroutine(Snipe());
  }

  private IEnumerator Snipe() {
    yield return new WaitForSeconds(_timeBeforeShot);

    OnRifleShot?.Invoke(this, EventArgs.Empty);

    Vector2 direction = VectorHelper.GetVectorToPoint(transform.position, _playerTransform.position);

    GameObject bulletObject = Instantiate(_bulletObject, _shootPoint.position, Quaternion.identity);
    SausageBullet bullet = bulletObject?.GetComponent<SausageBullet>();

    bullet.SetDirection(direction, Vector2.SignedAngle(Vector2.up, direction));

    _laserObject.SetActive(false);

    _anim.SetBool("IsSniping", false);

    OnAttackEnd?.Invoke(this, EventArgs.Empty);
  }
}
