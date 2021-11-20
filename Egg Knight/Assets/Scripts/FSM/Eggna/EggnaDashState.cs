using System;
using UnityEngine;

public class EggnaDashState : StateMachineBehaviour {
  public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    EggnaDash eggnaDash = animator.GetComponent<EggnaDash>();
    eggnaDash.StartDash();
  }
}
