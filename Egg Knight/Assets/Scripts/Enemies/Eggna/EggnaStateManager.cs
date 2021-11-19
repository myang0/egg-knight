using System;
using System.Collections;
using UnityEngine;

public class EggnaStateManager : MonoBehaviour {
  [SerializeField] private float _initialIdleTime;
  private float _currentIdleTime;
  public event EventHandler OnIdleEnd;

  private EnemyHealth _eHealth;

  private void Awake() {
    _currentIdleTime = _initialIdleTime;

    _eHealth = GetComponent<EnemyHealth>();
  }

  private void HandleEggnaActivate(object sender, EventArgs e) {
    if (GameObject.Find("Flowchart")) {
      Fungus.Flowchart.BroadcastFungusMessage("LadyEggnaStart");
    } else {
      GetComponent<Animator>().SetBool("IsActive", true);
    }
  }

  public void StartIdle() {
    StartCoroutine(Idle());
  }

  private IEnumerator Idle() {
    _currentIdleTime = _eHealth.CurrentHealthPercentage() * _initialIdleTime;

    yield return new WaitForSeconds(_currentIdleTime);

    OnIdleEnd?.Invoke(this, EventArgs.Empty);
  }
}
