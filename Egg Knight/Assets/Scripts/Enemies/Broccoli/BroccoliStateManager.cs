using System;
using System.Collections;
using UnityEngine;

public class BroccoliStateManager : MonoBehaviour {
  [SerializeField] private float _idleTime;
  public event EventHandler OnIdleEnd;

  [SerializeField] private float _spinTime;
  public event EventHandler OnSpinEnd;

  [SerializeField] private float _walkTime;
  public event EventHandler OnWalkEnd;

  [SerializeField] private float _parryTime;
  public event EventHandler OnParryEnd;

  [SerializeField] private float _chargeTime;
  public event EventHandler OnChargeEnd;

  private Animator _anim;
  private EnemyBehaviour _eBehaviour;

  private void Awake() {
    _anim = GetComponent<Animator>();

    _eBehaviour = GetComponent<EnemyBehaviour>();
  }

  public void StartIdle() {
    StartCoroutine(Idle());
  }

  private IEnumerator Idle() {
    yield return new WaitForSeconds(_idleTime);

    OnIdleEnd?.Invoke(this, EventArgs.Empty);
  }

  public void StartSpin() {
    StartCoroutine(Spin());
  }

  private IEnumerator Spin() {
    yield return new WaitForSeconds(_spinTime);

    OnSpinEnd?.Invoke(this, EventArgs.Empty);
  }
}
