using System;
using UnityEngine;

public class SausageParty : StateMachineBehaviour {
  public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    animator.SetBool("IsPartying", false);
  }
}
