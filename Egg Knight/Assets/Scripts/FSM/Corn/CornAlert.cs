using UnityEngine;

public class CornAlert : StateMachineBehaviour
{
  private EnemyBehaviour _eBehavior;
  override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    _eBehavior = animator.GetComponent<EnemyBehaviour>();
  }

  override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
  {
    if (_eBehavior.isDead) animator.SetTrigger("triggerDead");
    if (_eBehavior.isStunned) animator.SetBool("isStunned", true);

    _eBehavior.StopMoving();
    
    if (_eBehavior.isAttackOffCooldown) {
      animator.SetBool("isAttackReady", true);
    } 
  }

  override public void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex) {
    animator.SetBool("isAlert", false);
  }
}
