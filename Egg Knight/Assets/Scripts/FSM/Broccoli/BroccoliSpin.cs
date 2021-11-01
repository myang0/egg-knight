using System;
using UnityEngine;

public class BroccoliSpin : StateMachineBehaviour {
  private BroccoliStateManager _bStateManager;

  private Rigidbody2D _rb;
  private Animator _anim;

  public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    _bStateManager = animator.GetComponent<BroccoliStateManager>();

    _rb = animator.GetComponent<Rigidbody2D>();

    _anim = animator;
    _anim.SetBool("IsSpinning", true);

    _bStateManager.StartSpin();
    _bStateManager.OnSpinEnd += HandleSpinEnd;
  }

  private void HandleSpinEnd(object sender, EventArgs e) {
    _rb.velocity = Vector2.zero;
    _anim.SetBool("IsSpinning", false);
  }
}
