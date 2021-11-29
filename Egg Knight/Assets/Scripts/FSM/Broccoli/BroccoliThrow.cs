using UnityEngine;

public class BroccoliThrow : StateMachineBehaviour {
  private void OnStageEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex) {
    BroccoliStateManager bStateManager = animator.GetComponent<BroccoliStateManager>();
    bStateManager.StartThrow();
  }

  private void OnStageExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex) {
    BroccoliStateManager bStateManager = animator.GetComponent<BroccoliStateManager>();
    bStateManager.StopThrow();
  }
}
