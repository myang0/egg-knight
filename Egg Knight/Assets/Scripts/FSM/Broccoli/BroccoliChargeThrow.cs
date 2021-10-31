using UnityEngine;

public class BroccoliChargeThrow : StateMachineBehaviour {
  private void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex) {
    animator.SetBool("IsChargingThrow", false);  
  }
}
