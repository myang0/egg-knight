using UnityEngine;

public class EggArcherBow : MonoBehaviour {
  [SerializeField] private GameObject _arrowObject;

  private SpriteRenderer _bowSr;
  private Animator _bowAnim;

  private Transform _playerTransform;

  private void Awake() {
    _bowSr = GetComponent<SpriteRenderer>();
    _bowAnim = GetComponent<Animator>();

    _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
  }

  public void StartAttack() {
    _bowSr.enabled = true;
    _bowAnim.SetBool("IsShooting", true);
  }

  public void ShootArrow() {
    Vector2 vectorToPlayer = VectorHelper.GetVectorToPoint(transform.position, _playerTransform.position);
    float angleToPlayer = Vector2.SignedAngle(Vector2.up, vectorToPlayer);

    GameObject arrowObject = Instantiate(_arrowObject, transform.position, Quaternion.identity);
    arrowObject.GetComponent<Arrow>().SetDirection(vectorToPlayer, angleToPlayer);
  }

  public void EndAttack() {
    _bowSr.enabled = false;
    _bowAnim.SetBool("IsShooting", false);
  }
}
