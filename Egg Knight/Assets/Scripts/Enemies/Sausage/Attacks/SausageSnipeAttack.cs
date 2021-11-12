using System.Collections;
using UnityEngine;

public class SausageSnipeAttack : MonoBehaviour {
  [SerializeField] private float _timeBeforeShot;

  [SerializeField] private GameObject _bulletObject;
  [SerializeField] private GameObject _laserObject;

  private Transform _playerTransform;

  private void Awake() {
    _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

    StartAttack();
  }

  public void StartAttack() {
    _laserObject.SetActive(true);

    StartCoroutine(Snipe());
  }

  private IEnumerator Snipe() {
    yield return new WaitForSeconds(_timeBeforeShot);

    Vector2 direction = VectorHelper.GetVectorToPoint(transform.position, _playerTransform.position);

    GameObject bulletObject = Instantiate(_bulletObject, transform.position, Quaternion.identity);
    SausageBullet bullet = bulletObject?.GetComponent<SausageBullet>();

    bullet.SetDirection(direction, Vector2.SignedAngle(Vector2.up, direction));

    _laserObject.SetActive(false);
  }
}
