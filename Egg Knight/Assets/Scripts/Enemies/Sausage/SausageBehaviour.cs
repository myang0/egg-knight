using System;
using System.Collections;
using UnityEngine;

public class SausageBehaviour : EnemyBehaviour {
  [SerializeField] private float _minRangeToSnipe;

  protected override void Awake() {
    SausageHealth sausageHealth = GetComponent<SausageHealth>();
    sausageHealth.OnSausageStatusDamage += HandleStatusDamage;

    EnemyBehaviour enemyBehaviour = GetComponent<EnemyBehaviour>();
    enemyBehaviour.OnElectrocuted += HandleElectrocuted;

    Health = sausageHealth;
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

  public bool IsInSnipingRange() {
    return GetDistanceToPlayer() > _minRangeToSnipe;
  }
}
