using System;
using System.Collections;
using UnityEngine;

public class SausageStateManager : MonoBehaviour {
  [SerializeField] private float _minIdleTime;
  [SerializeField] private float _initialIdleTime;
  private float _currentIdleTime;
  public event EventHandler OnIdleEnd;

  private EnemyHealth _eHealth;

  private void Awake() {
    _currentIdleTime = _initialIdleTime;

    SausageActivator.OnSausageActivate += HandleSausageActivate;

    _eHealth = GetComponent<EnemyHealth>();
  }

  private void HandleSausageActivate(object sender, EventArgs e) {
    if (GameObject.Find("Flowchart")) {
      Fungus.Flowchart.BroadcastFungusMessage("SheriffSausageStart");
    } else {
      GetComponent<Animator>().SetBool("IsActive", true);
    }
  }

  public void StartIdle() {
    StartCoroutine(Idle());
  }

  private IEnumerator Idle() {
    _currentIdleTime = _minIdleTime + (_eHealth.CurrentHealthPercentage() * (_initialIdleTime - _minIdleTime));

    yield return new WaitForSeconds(_currentIdleTime);

    OnIdleEnd?.Invoke(this, EventArgs.Empty);
  }

  private void OnDestroy() {
    SausageActivator.OnSausageActivate -= HandleSausageActivate;
  }
}
