using System;
using System.Collections;
using UnityEngine;

public class BroccoliStateManager : MonoBehaviour {
  [SerializeField] private float _initialIdleTime;
  private float _currentIdleTime;
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
  private EnemyHealth _eHealth;

  private void Awake() {
    _currentIdleTime = _initialIdleTime;

    _anim = GetComponent<Animator>();

    _eBehaviour = GetComponent<EnemyBehaviour>();
    _eHealth = GetComponent<EnemyHealth>();
  }

  public void StartIdle() {
    StartCoroutine(Idle());
  }

  private IEnumerator Idle() {
    _currentIdleTime = _eHealth.CurrentHealthPercentage() * _initialIdleTime;

    yield return new WaitForSeconds(_currentIdleTime);

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
    yield return new WaitForSeconds(_walkTime);

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
