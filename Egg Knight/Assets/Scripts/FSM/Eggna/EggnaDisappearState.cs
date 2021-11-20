using System;
using UnityEngine;

public class EggnaDisappearState : StateMachineBehaviour {
  public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //  TODO: Move to animation event
    EggnaTeleport eggnaTeleport = animator.GetComponent<EggnaTeleport>();
    eggnaTeleport.Disappear();
  }

  public void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex) {
    animator.SetBool("IsDisappearing", false);
  }
}
