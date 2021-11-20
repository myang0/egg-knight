using System;
using UnityEngine;

public class EggnaThrowState : StateMachineBehaviour {
  public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    EggnaThrow eggnaThrow = animator.GetComponent<EggnaThrow>();
    eggnaThrow.StartAttack();
  }

  public void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex) {
    animator.SetBool("IsThrowing", false);
  }
}
