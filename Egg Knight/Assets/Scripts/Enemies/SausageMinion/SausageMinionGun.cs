using System.Collections;
using UnityEngine;

public class SausageMinionGun : MonoBehaviour {
  [SerializeField] private GameObject _bulletObject;

  [SerializeField] private float _timeBetweenShots;

  private Transform _playerTransform;

  private void Awake() {
    _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

    StartCoroutine(Shoot());
  }

  private void FixedUpdate() {
    float angleToPlayer = Vector2.SignedAngle(Vector2.up, GetVectorToPlayer());

    transform.eulerAngles = new Vector3(0, 0, angleToPlayer);
  }

  private IEnumerator Shoot() {
    while(true) {
      yield return new WaitForSeconds(_timeBetweenShots);

      GameObject bulletObject = Instantiate(_bulletObject, transform.position, Quaternion.identity);
      SausageBullet bullet = bulletObject.GetComponent<SausageBullet>();

      bullet.SetDirection(GetVectorToPlayer(), transform.eulerAngles.z);
    }
  }

  private Vector2 GetVectorToPlayer() {
    return VectorHelper.GetVectorToPoint(transform.position, _playerTransform.position);
  }
}
