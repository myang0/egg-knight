using System;
using System.Collections;
using UnityEngine;

public class EggnaBehaviour : EnemyBehaviour {
  [SerializeField] private float _meleeRange;
  [SerializeField] private float _longRange;

  protected override void Awake() {
    EggnaHealth eggnaHealth = GetComponent<EggnaHealth>();
    eggnaHealth.OnEggnaStatusDamage += HandleStatusDamage;

    EnemyBehaviour enemyBehaviour = GetComponent<EnemyBehaviour>();
    enemyBehaviour.OnElectrocuted += HandleElectrocuted;

    Health = eggnaHealth;
    base.Awake();
  }

  private void HandleElectrocuted(object sender, EventArgs e) {
    StartCoroutine(Electrocute());
  }

  protected override IEnumerator Electrocute() {
    yield break;
  }

  protected override IEnumerator AttackPlayer() {
    yield break;
  }

  public bool IsInMeleeRange() {
    return GetDistanceToPlayer() <= _meleeRange;
  }

  public bool IsInLongRange() {
    return GetDistanceToPlayer() > _longRange;
  }
}
