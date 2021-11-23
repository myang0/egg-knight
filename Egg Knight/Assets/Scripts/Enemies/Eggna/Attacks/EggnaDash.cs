using System.Collections;
using UnityEngine;

public class EggnaDash : MonoBehaviour {
  [SerializeField] private float _dashSpeed;
  private bool _isDashing = false;

  [SerializeField] private GameObject _echoObject;
  [SerializeField] private float _timeBetweenEchoSpawns;

  [SerializeField] private Transform _dashSlashPivot;
  [SerializeField] private Transform _dashSlashPoint;

  [SerializeField] private Transform _dashSlashObject;

  private Animator _anim;
  private Rigidbody2D _rb;
  private Collider2D _collider;

  private Transform _playerTransform;
  private Vector3 _targetPoint;

  private void Awake() {
    _anim = GetComponent<Animator>();
    _rb = GetComponent<Rigidbody2D>();
    _collider = GetComponent<Collider2D>();

    _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
  }

  private void FixedUpdate() {
    if (_isDashing) {
      transform.position = Vector3.MoveTowards(transform.position, _targetPoint, _dashSpeed);

      if (Vector3.Distance(transform.position, _targetPoint) < 0.01f) {
        StopDash();
      }
    }
  }

  public void StartDash() {
    _isDashing = true;
    _collider.enabled = false;
    _targetPoint = _playerTransform.position;

    StartCoroutine(DashTrail());
  }

  private IEnumerator DashTrail() {
    while (_isDashing) {
      yield return new WaitForSeconds(_timeBetweenEchoSpawns);

      Instantiate(_echoObject, transform.position, Quaternion.identity);
    }
  }

  private void StopDash() {
    _isDashing = false;
    _collider.enabled = true;
    _rb.velocity = Vector2.zero;

    StopCoroutine(DashTrail());

    _anim.SetBool("IsDashing", false);
  }

  public void StartDashSlash() {
    _dashSlashPivot.eulerAngles = new Vector3(0, 0, Vector2.SignedAngle(Vector2.up, GetVectorToPlayer()));

    if (_dashSlashObject != null) {
      Instantiate(_dashSlashObject, _dashSlashPoint.position, Quaternion.identity);
    }
  }

  private Vector2 GetVectorToPlayer() {
    return VectorHelper.GetVectorToPoint(transform.position, _playerTransform.position);
  }
}
