using System;
using System.Collections;
using UnityEngine;

public class SausageMinionBehaviour : EnemyBehaviour {
  [SerializeField] private GameObject _confettiObject;

  protected override void Awake() {
    SausageMinionHealth sausageMinionHealth = GetComponent<SausageMinionHealth>();
    sausageMinionHealth.OnSausageMinionStatusDamage += HandleStatusDamage;

    EnemyBehaviour enemyBehaviour = GetComponent<EnemyBehaviour>();
    enemyBehaviour.OnElectrocuted += HandleElectrocuted;

    if (_confettiObject) {
      Instantiate(_confettiObject, transform.position, Quaternion.identity);
    }

    Health = sausageMinionHealth;
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
}
