using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class BroccoliIdle : StateMachineBehaviour {
  private BroccoliBehaviour _bBehaviour;
  private BroccoliStateManager _bStateManager;

  private Animator _anim;

  public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    _bBehaviour = animator.GetComponent<EnemyBehaviour>() as BroccoliBehaviour;
    _bStateManager = animator.GetComponent<BroccoliStateManager>();

    _anim = animator;

    Rigidbody2D rb = animator.GetComponent<Rigidbody2D>();
    rb.velocity = Vector2.zero;

    _bStateManager.StartIdle();
    _bStateManager.OnIdleEnd += HandleIdleEnd;
  }

  private void HandleIdleEnd(object sender, EventArgs e) {
    if (_bBehaviour.IsInMeleeRange()) {
      ChooseCloseRangeAttack();
    } else {
      ChooseLongRangeAttack();
    }
  }

  private void ChooseCloseRangeAttack() {
    float attackRoll = Random.Range(0f, 1f);

    if (attackRoll < 0.33f) {
      _anim.SetBool("IsWalking", true);
    } else if (attackRoll >= 0.33f && attackRoll < 0.66f) {
      _anim.SetBool("IsChargingSlash", true);
    } else {
      _anim.SetBool("IsParrying", true);
    }
  }

  private void ChooseLongRangeAttack() {
    float attackRoll = Random.Range(0f, 1f);

    if (attackRoll < 0.5f) {
      _anim.SetBool("IsChargingSpin", true);
    } else {
      _anim.SetBool("IsChargingThrow", true);
    }
  }
}
