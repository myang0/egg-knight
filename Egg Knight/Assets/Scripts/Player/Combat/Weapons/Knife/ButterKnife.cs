using System;
using System.Collections;
using UnityEngine;

public class ButterKnife : BasePlayerWeapon {
  [SerializeField] private Transform _attackPoint;
  [SerializeField] private float _attackRange;

  [SerializeField] private Transform _finisherPoint;
  [SerializeField] private Vector2 _finisherRange;
  [SerializeField] private float _finisherDamage;

  [SerializeField] private LayerMask _obstacleLayer;

  [SerializeField] private AudioClip _swingClip;

  [SerializeField] private GameObject _knifeBeamPrefab;
  private bool _isKnifeBeam;
  public bool IsKnifeBeam {
    set {
      _isKnifeBeam = value;
    }
  }

  private bool _isAttacking = true;

  [SerializeField] private string[] _animatorStates;
  [SerializeField] private string[] _animatorParams;

  private int _attackIndex = 0;
  private int _maxAttackIndex = 2;

  protected override void Awake() {
    PlayerControls.OnLeftClick += HandleLeftClick;

    base.Awake();
  }

  private void HandleLeftClick(object sender, EventArgs e) {
    if (_isAttacking == false && _attackIndex < _maxAttackIndex) {
      if (_anim.GetCurrentAnimatorStateInfo(0).IsName(_animatorStates[_attackIndex])) {
        _anim.SetBool(_animatorParams[_attackIndex], true);

        ResetRotation();
        _isAttacking = true;

        _attackIndex++;
      }
    }
  }

  public override void EnableHitbox() {
    PlaySound(_swingClip);

    Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(_attackPoint.position, _attackRange, _enemyLayer);
    Collider2D[] obstaclesInRange = 
      Physics2D.OverlapCircleAll(_attackPoint.position, _attackRange, _obstacleLayer);
    
    Collider2D[] enemiesHit = new Collider2D[enemiesInRange.Length + obstaclesInRange.Length];
    enemiesInRange.CopyTo(enemiesHit, 0);
    obstaclesInRange.CopyTo(enemiesHit, enemiesInRange.Length);
    
    Collider2D[] coinsInRange = Physics2D.OverlapCircleAll(_attackPoint.position, _attackRange, _coinLayer);

    DamageEnemies(enemiesHit, _damageAmount, false);
    CollectCoins(coinsInRange);
  }

  public void FinisherHitbox() {
    PlaySound(_swingClip);

    if (_isKnifeBeam) {
      KnifeBeam knifeBeam = Instantiate(_knifeBeamPrefab, _attackPoint.position, Quaternion.identity).GetComponent<KnifeBeam>();
      knifeBeam.SetDirection(transform.up, transform.eulerAngles.z);
    }

    float hitboxAngle = transform.eulerAngles.z;

    Collider2D[] enemiesInRange = Physics2D.OverlapBoxAll(_finisherPoint.position, new Vector2(_finisherRange.x, _finisherRange.y), hitboxAngle, _enemyLayer);
    Collider2D[] obstaclesInRange = Physics2D.OverlapBoxAll(_finisherPoint.position, new Vector2(_finisherRange.x, _finisherRange.y), hitboxAngle, _obstacleLayer);
    Collider2D[] enemiesHit = new Collider2D[enemiesInRange.Length + obstaclesInRange.Length];

    enemiesInRange.CopyTo(enemiesHit, 0);
    obstaclesInRange.CopyTo(enemiesHit, enemiesInRange.Length);
    
    Collider2D[] coinsInRange = Physics2D.OverlapBoxAll(_finisherPoint.position, new Vector2(_finisherRange.x, _finisherRange.y), hitboxAngle, _coinLayer);

    DamageEnemies(enemiesHit, _finisherDamage, true);
    CollectCoins(coinsInRange);
  }

  public override void OnAnimationEnd() {
    _isAttacking = false;

    StartCoroutine(Despawn());
  }

  private IEnumerator Despawn() {
    yield return new WaitForSeconds(0.5f / _speed);

    if (_isAttacking == false) {
      base.OnAnimationEnd();
    }
  }

  protected override void OnDrawGizmosSelected() {
    Gizmos.color = Color.red;
    Gizmos.DrawWireSphere(_attackPoint.position, _attackRange);
    Gizmos.DrawWireCube(_finisherPoint.position, new Vector3(_finisherRange.x, _finisherRange.y, 1));
  }

  public override void MultiplyDamage(float modifier) {
    _damageAmount *= modifier;
    _finisherDamage *= modifier;
  }

  private void OnDestroy() {
    PlayerControls.OnLeftClick -= HandleLeftClick;
  }
}
