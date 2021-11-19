using UnityEngine;

public class BaconAttacking : StateMachineBehaviour {
  override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    BaconAttack bAttack = animator.GetComponent<BaconAttack>();
    bAttack.SpawnOrb();
  }

  override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    animator.SetBool("IsAttacking", false);
  }
}
