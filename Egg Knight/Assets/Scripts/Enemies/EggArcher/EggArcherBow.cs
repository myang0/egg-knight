using UnityEngine;

public class EggArcherBow : MonoBehaviour {
  [SerializeField] private EnemyBehaviour _eBehaviour;
  [SerializeField] private GameObject _arrowObject;

  private SpriteRenderer _bowSr;
  private Animator _bowAnim;
  private LineRenderer _lr;

  private Transform _playerTransform;

  private void Awake() {
    _bowSr = GetComponent<SpriteRenderer>();
    _bowAnim = GetComponent<Animator>();
    _lr = GetComponent<LineRenderer>();

    _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
  }

  private void Update() {
    if (_lr.enabled) {
      Vector3 bowPos = transform.position;
      Vector3 playerPos = _playerTransform.position;

      _lr.SetPositions(new Vector3[] { transform.position, _playerTransform.position });

      RaycastHit2D firstHit = Physics2D.Raycast(
        bowPos,
        transform.up,
        (1 << LayerMask.NameToLayer("Player")) | (1 << LayerMask.NameToLayer("Obstacle"))
      );

      Vector3 endPos = firstHit.transform.gameObject.CompareTag("Player") ? playerPos : (Vector3)firstHit.point;

      _lr.SetPositions(new Vector3[] { bowPos, endPos });
    } 
  }

  public void StartAttack() {
    _lr.enabled = true;
    _bowSr.enabled = true;
    _bowAnim.SetBool("IsShooting", true);
  }

  public void ShootArrow() {
    Vector2 vectorToPlayer = VectorHelper.GetVectorToPoint(transform.position, _playerTransform.position);
    float angleToPlayer = Vector2.SignedAngle(Vector2.up, vectorToPlayer);

    Arrow arrow = Instantiate(_arrowObject, transform.position, Quaternion.identity).GetComponent<Arrow>();

    if (_eBehaviour != null) {
      ProjectileHelper.Refrigerate(_eBehaviour.PlayerInventory, arrow);
    }

    arrow.SetDirection(vectorToPlayer, angleToPlayer);
  }

  public void EndAttack() {
    _lr.enabled = false;
    _bowSr.enabled = false;
    _bowAnim.SetBool("IsShooting", false);
  }
}
