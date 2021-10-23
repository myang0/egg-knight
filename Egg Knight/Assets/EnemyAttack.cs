using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : StateMachineBehaviour
{
      private EnemyBehaviour _eBehavior;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        _eBehavior = animator.GetComponent<EnemyBehaviour>();
        _eBehavior.Attack();
        Debug.Log("attack state");
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_eBehavior.GetIsAttackOffCooldown()) {
            animator.SetBool("isAttackReady", false);
        }
    }
}
