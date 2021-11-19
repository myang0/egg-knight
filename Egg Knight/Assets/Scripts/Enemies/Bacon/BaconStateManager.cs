using System.Collections;
using UnityEngine;

public class BaconStateManager : MonoBehaviour {
  [SerializeField] private float _idleTime;

  private Animator _anim;

  private void Awake() {
    _anim = GetComponent<Animator>();
  }

  public void StartIdle() {
    StartCoroutine(Idle());
  }

  private IEnumerator Idle() {
    yield return new WaitForSeconds(_idleTime);

    _anim.SetBool("IsAttacking", true);
  }
}
