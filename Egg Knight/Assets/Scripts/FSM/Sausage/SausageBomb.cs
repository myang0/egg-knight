using System;
using UnityEngine;

public class SausageBomb : StateMachineBehaviour {
  public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    SausageBombAttack bombAttack = animator.GetComponent<SausageBombAttack>();
    bombAttack.StartAttack();
  }
}
