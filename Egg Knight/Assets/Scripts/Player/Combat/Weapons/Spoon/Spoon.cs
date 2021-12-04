using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spoon : BasePlayerWeapon {
  [SerializeField] private Transform _attackPoint;
  [SerializeField] private float _attackWidth;
  [SerializeField] private float _attackHeight;

  [SerializeField] private Transform _finisherPoint;
  [SerializeField] private float _finisherRange;
  [SerializeField] private float _finisherDamage;

  [SerializeField] private LayerMask _obstacleLayer;

  [SerializeField] private AudioClip _swingClip;

  private bool _isAttacking = true;

  protected override void Awake() {
    PlayerControls.OnLeftClick += HandleLeftClick;

    base.Awake();
  }

  private void HandleLeftClick(object sender, EventArgs e) {
    if (_isAttacking == false) {
      if (_anim.GetCurrentAnimatorStateInfo(0).IsName("SpoonHit1")) {
        _anim.SetBool("IsHit2", true);
        ResetRotation();
        _isAttacking = true;
      } else if (_anim.GetCurrentAnimatorStateInfo(0).IsName("SpoonHit2")) {
        _anim.SetBool("IsHit3", true);
        ResetRotation();
        _isAttacking = true;
      }
    }
  }

  public override void EnableHitbox() {
    PlaySound(_swingClip);

    float hitboxAngle = transform.eulerAngles.z;

    Collider2D[] enemiesInRange = Physics2D.OverlapBoxAll(_attackPoint.position, new Vector2(_attackWidth, _attackHeight), hitboxAngle, _enemyLayer);
    Collider2D[] obstaclesInRange = Physics2D.OverlapBoxAll(_attackPoint.position, new Vector2(_attackWidth, _attackHeight), hitboxAngle, _obstacleLayer);
    Collider2D[] coinsInRange = Physics2D.OverlapBoxAll(_attackPoint.position, new Vector2(_attackWidth, _attackHeight), hitboxAngle, _coinLayer);

    DamageEnemies(enemiesInRange, _damageAmount);
    DamageEnemies(obstaclesInRange, _damageAmount);
    CollectCoins(coinsInRange);
  }

  public void FinisherHitbox() {
    PlaySound(_swingClip);

    Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(_finisherPoint.position, _finisherRange, _enemyLayer);
    Collider2D[] obstaclesInRange = Physics2D.OverlapCircleAll(_finisherPoint.position, _finisherRange, _obstacleLayer);
    Collider2D[] coinsInRange = Physics2D.OverlapCircleAll(_finisherPoint.position, _finisherRange, _coinLayer);

    DamageEnemies(enemiesInRange, _finisherDamage);
    DamageEnemies(obstaclesInRange, _finisherDamage);
    CollectCoins(coinsInRange);
  }

  protected override void PlaySound(AudioClip clip) {
    SingleTimeSound sound = Instantiate(_singleTimeSound, transform.position, Quaternion.identity)
      .GetComponent<SingleTimeSound>();

    sound.ScaleVolume(2f);
    sound.ScalePitch(0.6f);
    sound.LoadClipAndPlay(clip);
  }

  protected override void OnDrawGizmosSelected() {
    Gizmos.color = Color.red;
    Gizmos.DrawWireCube(_attackPoint.position, new Vector3(_attackWidth, _attackHeight, 1));
    Gizmos.DrawWireSphere(_finisherPoint.position, _finisherRange);
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

  private void OnDestroy() {
    PlayerControls.OnLeftClick -= HandleLeftClick;
  }
}
