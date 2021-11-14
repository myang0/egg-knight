using System;
using UnityEngine;

public class SausageWalk : StateMachineBehaviour {
  public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    SausageWalkAttack walkAttack = animator.GetComponent<SausageWalkAttack>();
    walkAttack.StartAttack();
  }
}
