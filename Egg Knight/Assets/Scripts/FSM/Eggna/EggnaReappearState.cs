using System;
using UnityEngine;

public class EggnaReappearState : StateMachineBehaviour {
  public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    EggnaTeleport eggnaTeleport = animator.GetComponent<EggnaTeleport>();
    eggnaTeleport.Reappear();
  }
}
