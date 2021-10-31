using UnityEngine;

public class BroccoliChargeSlash : StateMachineBehaviour {
  private void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex) {
    animator.SetBool("IsChargingSlash", false);  
  }
}
