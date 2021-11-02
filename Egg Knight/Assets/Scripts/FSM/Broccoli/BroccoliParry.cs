using System;
using UnityEngine;

public class BroccoliParry : StateMachineBehaviour {
  private BroccoliStateManager _bStateManager;

  private Animator _anim;

  public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    _bStateManager = animator.GetComponent<BroccoliStateManager>();

    _anim = animator;

    _bStateManager.StartParry();
    _bStateManager.OnParryEnd += HandleParryEnd;
  }

  public void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex) {
    _bStateManager.StopParry();

    _anim.SetBool("IsParrying", false);
  }

  private void HandleParryEnd(object sender, EventArgs e) {
    _anim.SetBool("IsParrying", false);
  }
}
