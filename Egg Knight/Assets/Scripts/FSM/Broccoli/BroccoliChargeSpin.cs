using UnityEngine;

public class BroccoliChargeSpin : StateMachineBehaviour {
  private void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex) {
    animator.SetBool("IsChargingSpin", false);  
  }
}
