using UnityEngine;

public class RoyalEggAttack : MonoBehaviour {
  [SerializeField] private GameObject _slashObject;

  private RoyalEggHealth _reHealth;
  private Transform _playerTransform;

  private void Awake() {
    _reHealth = GetComponent<RoyalEggHealth>();
    _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
  }

  public void StartAttack() {
    if (_slashObject != null) {
      _reHealth.isInvulnerable = false;
      CreateSlashHitbox();
    }
  }

  private void CreateSlashHitbox() {
    GameObject slashObject = Instantiate(_slashObject, transform.position, Quaternion.identity);

    Vector2 vectorToPlayer = VectorHelper.GetVectorToPoint(transform.position, _playerTransform.position);
    float angleToPlayer = Vector2.SignedAngle(Vector2.up, vectorToPlayer);

    RoyalEggSlash slash = slashObject.GetComponent<RoyalEggSlash>();
    slash?.SetRotation(angleToPlayer);
  }
}
