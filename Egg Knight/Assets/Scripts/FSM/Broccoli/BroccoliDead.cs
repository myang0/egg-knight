using UnityEngine;

public class BroccoliDead : StateMachineBehaviour {
  private void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex) {
    foreach(AnimatorControllerParameter parameter in animator.parameters) {
      animator.SetBool(parameter.name, false);
    }

    animator.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
  }
}
