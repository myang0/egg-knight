using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggtonioCreditsMoveBackwards: StateMachineBehaviour {
    private EggtonioCredits _ec;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        _ec = animator.GetComponent<EggtonioCredits>();
        _ec.isFastWobble = true;
        _ec.StartWobble();
        _ec.StartMoveToHideSpot();
        animator.ResetTrigger("NextState");
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (_ec.transform.position == _ec.hideSpot.position) animator.SetTrigger("NextState"); 
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        _ec.isFastWobble = false;
        _ec.StartStopWobble();
        animator.ResetTrigger("NextState");
        _ec.InvokeOnOutroEnd();
    }
}
