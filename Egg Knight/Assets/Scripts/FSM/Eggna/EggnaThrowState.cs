using System;
using UnityEngine;

public class EggnaThrowState : StateMachineBehaviour {
  public void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex) {
    animator.SetBool("IsThrowing", false);
  }
}
