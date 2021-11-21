using System;
using System.Collections;
using Stage;
using UnityEngine;

public class CornBehaviour : EnemyBehaviour {
  [SerializeField] private GameObject _kernelPrefab;

  [SerializeField] private int _numKernelsPerBurst;

  protected override void Awake() {
    CornHealth cornHealth = gameObject.GetComponent<CornHealth>();
    cornHealth.OnCornStatusDamage += HandleStatusDamage;

    EnemyBehaviour enemyBehaviour = gameObject.GetComponent<EnemyBehaviour>();
    enemyBehaviour.OnElectrocuted += HandleElectrocuted;

    attackCooldownMax = 6;
    maxDistanceToAttack = float.MaxValue;

    Health = cornHealth;
    isWallCollisionOn = true;
    base.Awake();
  }

  private void HandleElectrocuted(object sender, EventArgs e) {
    StartCoroutine(Electrocute());
  }

  public override void Attack() {
    StartCoroutine(AttackPlayer());
  }

  private IEnumerator AttackCooldown() {
    isAttackOffCooldown = false;
    yield return new WaitForSeconds(attackCooldownMax);
    isAttackOffCooldown = true;
  }

  protected override IEnumerator AttackPlayer() {
    rb.velocity = Vector2.zero;

    isInAttackAnimation = true;
    
    yield break;
  }

  public void ShootKernels() {
    for (int i = 0; i < _numKernelsPerBurst; i++) {
      Instantiate(_kernelPrefab, transform.position, Quaternion.identity);
    }

    isInAttackAnimation = false;
    StartCoroutine(AttackCooldown());
  }
}
