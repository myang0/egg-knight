using System;
using UnityEngine;

public class SausageParty : StateMachineBehaviour {
  public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    SausagePartyAttack partyAttack = animator.GetComponent<SausagePartyAttack>();
    partyAttack.StartAttack();
  }

  public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    animator.SetBool("IsPartying", false);
  }
}
