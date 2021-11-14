using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class SausageIdle : StateMachineBehaviour {
  private SausageBehaviour _sBehaviour;
  private SausageStateManager _sStateManager;

  private Animator _anim;

  private static int _subscribers = 0;

  public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    _sBehaviour = animator.GetComponent<EnemyBehaviour>() as SausageBehaviour;
    _sStateManager = animator.GetComponent<SausageStateManager>();

    _anim = animator;

    foreach(AnimatorControllerParameter parameter in _anim.parameters) {
      _anim.SetBool(parameter.name, false);
    }

    _anim.SetBool("IsActive", true);

    Rigidbody2D rb = animator.GetComponent<Rigidbody2D>();
    rb.velocity = Vector2.zero;

    _sStateManager.StartIdle();
    
    if (_subscribers == 0) {
      _sStateManager.OnIdleEnd += HandleIdleEnd;
      _subscribers++;
    }
  }

  private void HandleIdleEnd(object sender, EventArgs e) {
    if (_sBehaviour.IsInSnipingRange()) {
      ChooseLongRangeAttack();
    } else {
      ChooseCloseRangeAttack();
    }
  }

  private void ChooseLongRangeAttack() {
    int attackRoll = Random.Range(0, 100);

    if (attackRoll < 75) {
      _anim.SetBool("IsSniping", true);
    } else {
      ChooseCloseRangeAttack();
    }
  }

  private void ChooseCloseRangeAttack() {
    int attackRoll = Random.Range(0, 100);

    if (attackRoll < 25) {
      _anim.SetBool("IsBombing", true);
    } else if (attackRoll >= 25 && attackRoll < 50) {
      _anim.SetBool("IsWalking", true);
    } else if (attackRoll >= 50 && attackRoll < 75) {
      _anim.SetBool("IsPartying", true);
    } else {
      _anim.SetBool("IsSpinning", true);
    }
  }

  private void OnDestroy() {
    if (_subscribers > 0) {
      _sStateManager.OnIdleEnd -= HandleIdleEnd;
      _subscribers = 0;
    }
  }
}
