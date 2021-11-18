using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggArcherFleeing : StateMachineBehaviour
{
  [SerializeField] private float _safeDistance;

  private EggArcherBehaviour _eaBehaviour;

  override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    animator.SetBool("IsFleeing", true);

    _eaBehaviour = animator.GetComponent<EggArcherBehaviour>();
  }

  override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    if (_eaBehaviour.isDead) animator.SetTrigger("triggerDead");
    if (_eaBehaviour.isStunned) animator.SetBool("isStunned", true);

    _eaBehaviour.Flee();

    if (_eaBehaviour.GetDistanceToPlayer() > _safeDistance) {
      animator.SetBool("IsFleeing", false);
    }
  }
}
