using System;
using System.Collections;
using UnityEngine;

public class BroccoliSpin : StateMachineBehaviour {
  private BroccoliStateManager _bStateManager;

  private Animator _anim;

  public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    _bStateManager = animator.GetComponent<BroccoliStateManager>();

    _anim = animator;
    _anim.SetBool("IsSpinning", true);

    _bStateManager.StartSpin();
    _bStateManager.OnSpinEnd += HandleSpinEnd;
  }

  private void HandleSpinEnd(object sender, EventArgs e) {
    _anim.SetBool("IsSpinning", false);
  }
}
