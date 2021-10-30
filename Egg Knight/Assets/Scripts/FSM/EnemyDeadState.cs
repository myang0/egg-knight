using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeadState : StateMachineBehaviour
{
    private EnemyBehaviour _eBehavior;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        _eBehavior = animator.GetComponent<EnemyBehaviour>();
        _eBehavior.rb.velocity = new Vector2(0, 0);
    }

}
