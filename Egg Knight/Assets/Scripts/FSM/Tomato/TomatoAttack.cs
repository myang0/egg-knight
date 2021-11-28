using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TomatoAttack : StateMachineBehaviour
{
  private EnemyBehaviour _eBehavior;
  private TomatoBehaviour _tBehavior;
  
  override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    _eBehavior = animator.GetComponent<EnemyBehaviour>();
    _eBehavior.Attack();
    animator.SetBool("isAttackReady", false);
  }

  override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    if (_eBehavior.isDead) animator.SetTrigger("triggerDead");
    if (_eBehavior.isStunned) animator.SetBool("isStunned", true);
  }

  public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    _eBehavior.isInAttackAnimation = false;
    _eBehavior.GetComponent<CircleCollider2D>().isTrigger = false;
  }
}
