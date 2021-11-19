using UnityEngine;

public class BaconTeleportingOut : StateMachineBehaviour {
  override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    BaconTeleport bTeleport = animator.GetComponent<BaconTeleport>();
    bTeleport.Disappear();
  }
}
