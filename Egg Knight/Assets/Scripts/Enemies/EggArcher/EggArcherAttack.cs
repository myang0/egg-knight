using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class EggArcherAttack : MonoBehaviour {
  [SerializeField] private GameObject _arrowObject;

  [SerializeField] private float _dangerRange;

  [SerializeField] private float _rollDuration;
  [SerializeField] private float _rollSpeed;
  [SerializeField] private float _rollDrag;

  private Animator _anim;
  private Rigidbody2D _rb;
  private EnemyBehaviour _eBehaviour;
  private EnemyHealth _eHealth;

  private Transform _playerTransform;

  private void Awake() {
    _anim = GetComponent<Animator>();
    _rb = GetComponent<Rigidbody2D>();
    _eBehaviour = GetComponent<EnemyBehaviour>();
    _eHealth = GetComponent<EnemyHealth>();

    _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

    PlayerControls.OnLeftClick += HandlePlayerAttack;
    PlayerControls.OnRightClick += HandlePlayerAttack;
  }

  private void HandlePlayerAttack(object sender, EventArgs e) {
    if (_eBehaviour.GetDistanceToPlayer() < _dangerRange && _anim.GetBool("IsAttacking")) {
      StartCoroutine(Roll());
    }
  }

  private IEnumerator Roll() {
    _eHealth.isInvulnerable = true;

    _anim.SetBool("IsRolling", true);

    Vector2 rollDirection = GetVectorToPlayer();

    _rb.velocity = rollDirection * _rollSpeed;
    _rb.drag = _rollDrag;

    yield return new WaitForSeconds(_rollDuration);

    _rb.velocity = Vector2.zero;
    _rb.drag = 0;

    _anim.SetBool("IsRolling", false);

    _eHealth.isInvulnerable = false;
  }

  public void ShootArrowAtPlayer() {
    Vector2 vectorToPlayer = GetVectorToPlayer();
    float angleToPlayer = Vector2.SignedAngle(Vector2.up, vectorToPlayer);

    GameObject arrowObject = Instantiate(_arrowObject, transform.position, Quaternion.identity);
    arrowObject.GetComponent<Arrow>().SetDirection(vectorToPlayer, angleToPlayer);
  }

  private Vector2 GetVectorToPlayer() {
    return VectorHelper.GetVectorToPoint(transform.position, _playerTransform.position);
  }

  private void OnDestroy() {
    PlayerControls.OnLeftClick -= HandlePlayerAttack;
    PlayerControls.OnRightClick -= HandlePlayerAttack;
  }
}
