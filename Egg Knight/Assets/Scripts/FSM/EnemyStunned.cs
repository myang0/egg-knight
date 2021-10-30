using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStunned : StateMachineBehaviour
{
    private EnemyBehaviour _eBehavior;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        _eBehavior = animator.GetComponent<EnemyBehaviour>();
    }
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_eBehavior.isDead) animator.SetTrigger("triggerDead");
        if (!_eBehavior.isStunned) animator.SetBool("isStunned", false);
    }
}
