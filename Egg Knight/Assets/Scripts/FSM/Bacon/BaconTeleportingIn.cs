using UnityEngine;

public class BaconTeleportingIn : StateMachineBehaviour {
  override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    BaconTeleport bTeleport = animator.GetComponent<BaconTeleport>();
    bTeleport.Reappear();
  }

  override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    animator.SetBool("IsTeleportingIn", false);
  }
}
