using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : StateMachineBehaviour
{
    private EnemyBehaviour _eBehavior;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        _eBehavior = animator.GetComponent<EnemyBehaviour>();
        _eBehavior.Attack();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_eBehavior.isDead) animator.SetTrigger("triggerDead");
        _eBehavior.StopMoving();
        if (_eBehavior.isStunned) animator.SetBool("isStunned", true);
        if (_eBehavior.isStunned) animator.SetBool("isAttackReady", false);
        if (!_eBehavior.isInAttackAnimation) {
            animator.SetBool("isAttackReady", false);
        }
    }
}
