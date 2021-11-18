using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggArcherAttacking : StateMachineBehaviour
{
    private EggArcherBehaviour _eaBehaviour;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        _eaBehaviour = animator.GetComponent<EggArcherBehaviour>();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_eaBehaviour.isDead) animator.SetTrigger("triggerDead");
        if (_eaBehaviour.isStunned) animator.SetBool("isStunned", true);

        _eaBehaviour.StopMoving();
        
        if (_eaBehaviour.HasClearShot() == false) {
            animator.SetBool("IsAttacking", false);
        }
    }
}
