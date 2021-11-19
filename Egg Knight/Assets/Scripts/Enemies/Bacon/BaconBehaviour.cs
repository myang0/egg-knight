using System;
using System.Collections;
using UnityEngine;

public class BaconBehaviour : EnemyBehaviour {
  protected override void Awake() {
    BaconHealth baconHealth = gameObject.GetComponent<BaconHealth>();
    baconHealth.OnBaconStatusDamage += HandleStatusDamage;

    EnemyBehaviour enemyBehaviour = gameObject.GetComponent<EnemyBehaviour>();
    enemyBehaviour.OnElectrocuted += HandleElectrocuted;

    Health = baconHealth;
    base.Awake();
  }

  private void HandleElectrocuted(object sender, EventArgs e) {
    StartCoroutine(Electrocute());
  }

  protected override IEnumerator AttackPlayer() {
    yield break;
  }
}
