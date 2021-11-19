using UnityEngine;

public class BaconIdle : StateMachineBehaviour {
  override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    BaconStateManager bStateManager = animator.GetComponent<BaconStateManager>();
    bStateManager.StartIdle();
  }
}
