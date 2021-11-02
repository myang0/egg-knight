using System;
using UnityEngine;

public class BroccoliChargeSlash : StateMachineBehaviour {
  private BroccoliStateManager _bStateManager;

  private Animator _anim;

  public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    _bStateManager = animator.GetComponent<BroccoliStateManager>();

    _anim = animator;

    _bStateManager.StartCharge();
    _bStateManager.OnChargeEnd += HandleChargeEnd;
  }

  private void HandleChargeEnd(object sender, EventArgs e) {
    _anim.SetBool("IsChargingSlash", false);
  }
}
