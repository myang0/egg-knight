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

  public void StartWalk() {
    StartCoroutine(Walk());
  }

  private IEnumerator Walk() {
    Debug.Log(_walkTime);

    yield return new WaitForSeconds(_walkTime);

    Debug.Log("end");

    OnWalkEnd?.Invoke(this, EventArgs.Empty);
  }

  public void StartParry() {
    StartCoroutine(Parry());
  }

  public void StopParry() {
    StopCoroutine(Parry());
  }

  private IEnumerator Parry() {
    yield return new WaitForSeconds(_parryTime);

    OnParryEnd?.Invoke(this, EventArgs.Empty);
  }

  public void StartCharge() {
    StartCoroutine(Charge());
  }

  private IEnumerator Charge() {
    yield return new WaitForSeconds(_chargeTime);

    OnChargeEnd?.Invoke(this, EventArgs.Empty);
  }
}
