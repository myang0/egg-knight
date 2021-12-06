using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class EggArcherAttack : MonoBehaviour {
  [SerializeField] private float _dangerRange;

  [SerializeField] private float _rollDuration;
  [SerializeField] private float _rollSpeed;
  [SerializeField] private float _rollDrag;

  private float _anglesPerFrame;
  private bool _isRolling = false;

  private Animator _anim;
  private Rigidbody2D _rb;
  private EggArcherBehaviour _eaBehaviour;
  private EnemyHealth _eHealth;

  private EggArcherBow _bow;

  private Transform _playerTransform;
  private bool _isRollReady = true;

  private void Awake() {
    _anim = GetComponent<Animator>();
    _rb = GetComponent<Rigidbody2D>();
    _eaBehaviour = GetComponent<EggArcherBehaviour>();
    _eHealth = GetComponent<EnemyHealth>();

    _bow = _eaBehaviour.Bow;

    _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

    PlayerControls.OnLeftClick += HandlePlayerAttack;
    PlayerControls.OnRightClick += HandlePlayerAttack;

    _anglesPerFrame = (360.0f / (50 * _rollDuration));
  }

  private void FixedUpdate() {
    if (_isRolling) {
      transform.Rotate(0, 0, _anglesPerFrame);
    }
  }

  private void HandlePlayerAttack(object sender, EventArgs e) {
    if (_eaBehaviour.GetDistanceToPlayer() < _dangerRange && _anim.GetBool("IsAttacking")) {
      if (_isRollReady) {
        _isRollReady = false;
        StartCoroutine(Roll());
        StartCoroutine(StartRollCD());
      }
    }
  }

  private IEnumerator StartRollCD() {
    yield return new WaitForSeconds(2.5f);
    _isRollReady = true;
  }

  private IEnumerator Roll() {
    _bow.EndAttack();
    
    _eHealth.isInvulnerable = true;

    _anim.SetBool("IsRolling", true);
    _isRolling = true;

    Vector2 rollDirection = GetVectorToPlayer();

    _rb.velocity = rollDirection * _rollSpeed;
    _rb.drag = _rollDrag;

    yield return new WaitForSeconds(_rollDuration);

    _rb.velocity = Vector2.zero;
    _rb.drag = 0;

    _anim.SetBool("IsRolling", false);
    _isRolling = false;

    transform.eulerAngles = Vector3.zero;

    _eHealth.isInvulnerable = false;
  }

  private Vector2 GetVectorToPlayer() {
    return VectorHelper.GetVectorToPoint(transform.position, _playerTransform.position);
  }

  private void OnDestroy() {
    PlayerControls.OnLeftClick -= HandlePlayerAttack;
    PlayerControls.OnRightClick -= HandlePlayerAttack;
  }
}
