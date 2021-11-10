using System;
using System.Collections;
using UnityEngine;

public class EnemySpriteHandler : MonoBehaviour {
  protected SpriteRenderer _sr;

  protected virtual void Awake() {
    _sr = gameObject.GetComponent<SpriteRenderer>();

    EnemyBehaviour enemyBehaviour = gameObject.GetComponent<EnemyBehaviour>();

    enemyBehaviour.OnYolked += (object sender, EventArgs e) => StartCoroutine(Yolk());
    enemyBehaviour.OnFrosted += (object sender, EventArgs e) => StartCoroutine(Frost());
    enemyBehaviour.OnIgnited += (object sender, EventArgs e) => StartCoroutine(Ignite());
    enemyBehaviour.OnElectrocuted += (object sender, EventArgs e) => StartCoroutine(Electrocute());
  }

  protected virtual IEnumerator Yolk() {
    _sr.color = new Color(1, 0.6f, 0.475f, 1);

    yield return new WaitForSeconds(StatusConfig.YolkedDuration);

    _sr.color = Color.white;
  }

  protected virtual IEnumerator Frost() {
    _sr.color = new Color(0.25f, 0.4f, 0.9f, 1);

    yield return new WaitForSeconds(StatusConfig.FrostDuration);

    _sr.color = Color.white;
  }

  protected virtual IEnumerator Ignite() {
    _sr.color = new Color(1, 0.4f, 0.25f, 1);

    yield return new WaitForSeconds(StatusConfig.IgniteDuration);

    _sr.color = Color.white;
  }

  protected virtual IEnumerator Electrocute() {
    _sr.color = new Color(1, 1, 0.4f, 1);

    yield return new WaitForSeconds(StatusConfig.ElectrocuteStunDuration);

    _sr.color = Color.white;
  }
}
