using System;
using System.Collections;
using UnityEngine;

public class TwoProngedFork : BasePlayerWeapon {
  [SerializeField] private Transform _attackPoint;
  [SerializeField] private Vector2 _attackRange;

  [SerializeField] private Transform _finisherPoint;
  [SerializeField] private Vector2 _finisherRange;
  [SerializeField] private float _finisherDamage;

  private int _attackIndex = 0;
  private int _maxAttackIndex = 3;

  [SerializeField] private string[] _animatorStates;
  [SerializeField] private string[] _animatorParams;

  private bool _isAttacking = true;

  [SerializeField] private LayerMask _obstacleLayer;

  [SerializeField] private AudioClip _useClip;

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

  }

  public void IntermediateHitbox(float modifierAngle) {
    PlaySound(_useClip);

    float hitboxAngle = transform.eulerAngles.z + modifierAngle;

    Collider2D[] enemiesInRange = Physics2D.OverlapBoxAll(_attackPoint.position, new Vector2(_attackRange.x, _attackRange.y), hitboxAngle, _enemyLayer);
    Collider2D[] obstaclesInRange = Physics2D.OverlapBoxAll(_attackPoint.position, new Vector2(_attackRange.x, _attackRange.y), hitboxAngle, _obstacleLayer);
    Collider2D[] enemiesHit = new Collider2D[enemiesInRange.Length + obstaclesInRange.Length];

    enemiesInRange.CopyTo(enemiesHit, 0);
    obstaclesInRange.CopyTo(enemiesHit, enemiesInRange.Length);
    
    Collider2D[] coinsInRange = Physics2D.OverlapBoxAll(_attackPoint.position, new Vector2(_attackRange.x, _attackRange.y), hitboxAngle, _coinLayer);

    DamageEnemies(enemiesHit, _damageAmount);
    CollectCoins(coinsInRange);
  }

  public void FinisherHitbox() {
    PlaySound(_useClip);

    float hitboxAngle = transform.eulerAngles.z;

    Collider2D[] enemiesInRange = Physics2D.OverlapBoxAll(_finisherPoint.position, new Vector2(_finisherRange.x, _finisherRange.y), hitboxAngle, _enemyLayer);
    Collider2D[] obstaclesInRange = Physics2D.OverlapBoxAll(_finisherPoint.position, new Vector2(_finisherRange.x, _finisherRange.y), hitboxAngle, _obstacleLayer);
    Collider2D[] enemiesHit = new Collider2D[enemiesInRange.Length + obstaclesInRange.Length];

    enemiesInRange.CopyTo(enemiesHit, 0);
    obstaclesInRange.CopyTo(enemiesHit, enemiesInRange.Length);
    
    Collider2D[] coinsInRange = Physics2D.OverlapBoxAll(_finisherPoint.position, new Vector2(_finisherRange.x, _finisherRange.y), hitboxAngle, _coinLayer);

    DamageEnemies(enemiesHit, _finisherDamage);
    CollectCoins(coinsInRange);
  }

  protected override void PlaySound(AudioClip clip) {
    SingleTimeSound sound = Instantiate(_singleTimeSound, transform.position, Quaternion.identity)
      .GetComponent<SingleTimeSound>();

    sound.ScalePitch(1.1f);
    sound.LoadClipAndPlay(clip);
  }

  public override void OnAnimationEnd() {
    _isAttacking = false;

    StartCoroutine(Despawn());
  }

  private IEnumerator Despawn() {
    yield return new WaitForSeconds(0.25f / _speed);

    if (_isAttacking == false) {
      base.OnAnimationEnd();
    }
  }

  protected override void OnDrawGizmosSelected() {
    Gizmos.color = Color.red;
    Gizmos.DrawWireCube(_attackPoint.position, new Vector3(_attackRange.x, _attackRange.y, 1));
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
