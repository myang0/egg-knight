using UnityEngine;

public class SniperLaser : MonoBehaviour {
  private LineRenderer _lr;

  private Transform _playerTransform;

  private void Awake() {
    _lr = GetComponent<LineRenderer>();

    _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
  }

  private void Update() {
    Vector2 vectorToPlayer = VectorHelper.GetVectorToPoint(transform.position, _playerTransform.position);
    transform.eulerAngles = new Vector3(0, 0, Vector2.SignedAngle(Vector2.up, vectorToPlayer));

    float distanceToPlayer = Vector2.Distance(transform.position, _playerTransform.position);

    Vector3 endPosition = transform.position + (transform.up * distanceToPlayer);
    _lr.SetPositions(new Vector3[] { transform.position, endPosition });
  }
}
