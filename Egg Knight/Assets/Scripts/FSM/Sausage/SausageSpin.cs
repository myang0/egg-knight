using System;
using UnityEngine;

public class SausageSpin : StateMachineBehaviour {
  public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    animator.SetBool("IsChargingSpin", false);
    animator.SetBool("IsSpinning", true);

    SausageSpinAttack spinAttack = animator.GetComponent<SausageSpinAttack>();
    spinAttack.StartAttack();
  }
}
