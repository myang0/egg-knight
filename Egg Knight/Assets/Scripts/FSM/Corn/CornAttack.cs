using UnityEngine;

public class CornAttack : StateMachineBehaviour
{
  override public void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex) {
    animator.SetBool("isFleeing", true);
  }
}
