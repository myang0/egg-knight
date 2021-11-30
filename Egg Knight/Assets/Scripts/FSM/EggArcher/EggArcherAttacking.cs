using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggArcherAttacking : StateMachineBehaviour
{
    private EggArcherBehaviour _eaBehaviour;
    private EggArcherBow _bow;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        _eaBehaviour = animator.GetComponent<EggArcherBehaviour>();
        _bow = _eaBehaviour.Bow;

        _eaBehaviour.StopMoving();

        _bow.StartAttack();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (_eaBehaviour.isDead) animator.SetTrigger("triggerDead");
        if (_eaBehaviour.isStunned) animator.SetBool("isStunned", true);
        
        if (_eaBehaviour.HasClearShot() == false) {
            Debug.Log("hi");
            animator.SetBool("IsAttacking", false);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        _bow.EndAttack();
    }
}
