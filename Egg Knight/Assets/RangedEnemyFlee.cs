using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemyFlee : StateMachineBehaviour
{
    private EnemyBehaviour _eBehavior;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        _eBehavior = animator.GetComponent<EnemyBehaviour>();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (_eBehavior.isStunned) animator.SetBool("isStunned", true);
        if (_eBehavior.isStunned) animator.SetBool("isFleeing", false);
        _eBehavior.Flee();
        if (_eBehavior.minDistanceToAttack+1 < _eBehavior.GetDistanceToPlayer() || _eBehavior.GetIsAttackReady()) {
            animator.SetBool("isFleeing", false);
        }
    }
}
