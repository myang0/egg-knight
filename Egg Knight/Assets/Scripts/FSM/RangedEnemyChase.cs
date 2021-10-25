using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemyChase : StateMachineBehaviour
{
    private EnemyBehaviour _eBehavior;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        _eBehavior = animator.GetComponent<EnemyBehaviour>();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_eBehavior.isStunned) animator.SetBool("isStunned", true);
        if (_eBehavior.GetIsAttackReady()) {
            animator.SetBool("isAttackReady", true);
            animator.SetBool("isChasing", false);
        } else if (_eBehavior.maxDistanceToAttack > _eBehavior.GetDistanceToPlayer()) {
            animator.SetBool("isChasing", false);
        } 
        _eBehavior.Move();
    }
}
