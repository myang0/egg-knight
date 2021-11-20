using System;
using UnityEngine;

public class EggnaSpinState : StateMachineBehaviour {
  public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    EggnaSpin eggnaSpin = animator.GetComponent<EggnaSpin>();
    eggnaSpin.StartAttack();
  }
}
