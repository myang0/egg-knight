using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemyAlerted : StateMachineBehaviour
{
    private EnemyBehaviour _eBehavior;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        _eBehavior = animator.GetComponent<EnemyBehaviour>();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_eBehavior.GetIsAttackReady()) {
            animator.SetBool("isAttackReady", true);
        }

        if (_eBehavior.maxDistanceToAttack < _eBehavior.GetDistanceToPlayer()) {
            _eBehavior.MoveToPlayer();
        }

        if (_eBehavior.minDistanceToAttack > _eBehavior.GetDistanceToPlayer()) {
            _eBehavior.Flee();
        }
    }
}
