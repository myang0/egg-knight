using System;
using UnityEngine;

public class BroccoliParry : StateMachineBehaviour {
  private BroccoliStateManager _bStateManager;

  private Animator _anim;

  private static int _subscribers = 0;

  public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    _bStateManager = animator.GetComponent<BroccoliStateManager>();

    _anim = animator;

    _bStateManager.StartParry();

    if (_subscribers <= 0) {
      _bStateManager.OnParryEnd += HandleParryEnd;
      _subscribers++;
    }
  }

  public void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex) {
    _bStateManager.StopParry();

    _anim.SetBool("IsParrying", false);
  }

  private void HandleParryEnd(object sender, EventArgs e) {
    _anim.SetBool("IsParrying", false);
  }

  private void OnDestroy() {
    if (_subscribers > 0) {
      _bStateManager.OnParryEnd -= HandleParryEnd;
      _subscribers = 0;
    }
  }
}
