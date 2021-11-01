using System;
using System.Collections;
using UnityEngine;

public class BroccoliBehaviour : EnemyBehaviour {
  [SerializeField] private float _meleeDistance;

  private string _name = "Brigand Broccoli";
  public static event EventHandler<BossSpawnEventArgs> OnBroccoliSpawn;

  protected override void Awake() {
    OnBroccoliSpawn?.Invoke(this, new BossSpawnEventArgs(_name));

    BroccoliHealth broccoliHealth = gameObject.GetComponent<BroccoliHealth>();
    broccoliHealth.OnBroccoliStatusDamage += HandleStatusDamage;

    EnemyBehaviour enemyBehaviour = gameObject.GetComponent<EnemyBehaviour>();
    enemyBehaviour.OnElectrocuted += HandleElectrocuted;

    Health = broccoliHealth;
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
    return GetDistanceToPlayer() <= _meleeDistance;
  }
}
