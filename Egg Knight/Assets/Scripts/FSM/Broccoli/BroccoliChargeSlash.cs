using System;
using UnityEngine;

public class BroccoliChargeSlash : StateMachineBehaviour {
  private BroccoliStateManager _bStateManager;

  private Animator _anim;

  private static int _subscribers = 0;

  public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    _bStateManager = animator.GetComponent<BroccoliStateManager>();

    _anim = animator;

    _bStateManager.StartCharge();

    if (_subscribers <= 0) {
      _bStateManager.OnChargeEnd += HandleChargeEnd;
      _subscribers++;
    }
  }

  private void HandleChargeEnd(object sender, EventArgs e) {
    _anim.SetBool("IsChargingSlash", false);
  }
}
