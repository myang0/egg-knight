using System;
using UnityEngine;

public class SausageSnipe : StateMachineBehaviour {
  public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    SausageSnipeAttack snipeAttack = animator.GetComponent<SausageSnipeAttack>();
    snipeAttack.StartAttack();
  }
}
