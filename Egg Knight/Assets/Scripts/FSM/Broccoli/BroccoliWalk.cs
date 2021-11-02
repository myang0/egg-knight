using System;
using UnityEngine;

public class BroccoliWalk : StateMachineBehaviour {
  private BroccoliStateManager _bStateManager;

  private Rigidbody2D _rb;
  private Animator _anim;

  private static int _subscribers = 0;

  public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    _bStateManager = animator.GetComponent<BroccoliStateManager>();

    _rb = animator.GetComponent<Rigidbody2D>();
    _anim = animator;

    _bStateManager.StartWalk();

    if (_subscribers <= 0) {
      _bStateManager.OnWalkEnd += HandleWalkEnd;
      _subscribers++;
    }
  }

  private void HandleWalkEnd(object sender, EventArgs e) {
    _rb.velocity = Vector2.zero;
    _anim.SetBool("IsWalking", false);
  }

  private void OnDestroy() {
    if (_subscribers > 0) {
      _bStateManager.OnWalkEnd -= HandleWalkEnd;
      _subscribers = 0;
    }
  }
}
