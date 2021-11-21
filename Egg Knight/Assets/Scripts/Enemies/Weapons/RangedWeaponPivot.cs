using UnityEngine;

public class RangedWeaponPivot : MonoBehaviour {
  private Transform _playerTransform;

  private void Awake() {
    _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
  }

  private void Update() {
    Vector2 vectorToPlayer = VectorHelper.GetVectorToPoint(transform.position, _playerTransform.position);
    transform.eulerAngles = new Vector3(0, 0, Vector2.SignedAngle(Vector2.up, vectorToPlayer));
  }
}
