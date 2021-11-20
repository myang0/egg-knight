using System;
using UnityEngine;

public class EggnaSlashState : StateMachineBehaviour {
  public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    EggnaSlash eggnaSlash = animator.GetComponent<EggnaSlash>();
    eggnaSlash.StartAttack();
  }
}
