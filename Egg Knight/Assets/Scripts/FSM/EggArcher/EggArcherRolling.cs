using UnityEngine;

public class EggArcherRolling : StateMachineBehaviour
{
  override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    animator.SetBool("IsAttacking", false);
  }
}
