using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoyalEggAlerted : StateMachineBehaviour
{
  private RoyalEggBehaviour _reBehaviour;

  override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    _reBehaviour = animator.GetComponent<RoyalEggBehaviour>();
    _reBehaviour.isWallCollisionOn = false;

    _reBehaviour.StartMinAlertTime();

    RoyalEggHealth reHealth = animator.GetComponent<RoyalEggHealth>();
    reHealth.isInvulnerable = true;
  }

  override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
  {
    if (_reBehaviour.isDead) animator.SetTrigger("triggerDead");
    if (_reBehaviour.isStunned) animator.SetBool("isStunned", true);
    else if (_reBehaviour.GetIsAttackReady()) {
      animator.SetBool("isAttackReady", true);
    }
    else {
      _reBehaviour.Move();
    }
  }
}
