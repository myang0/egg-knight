using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class EggnaIdle : StateMachineBehaviour {
  private EggnaBehaviour _eBehaviour;
  private EggnaStateManager _eStateManager;

  private Animator _anim;

  private static int _subscribers = 0;

  public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    _eBehaviour = animator.GetComponent<EnemyBehaviour>() as EggnaBehaviour;
    _eStateManager = animator.GetComponent<EggnaStateManager>();

    _anim = animator;

    foreach(AnimatorControllerParameter parameter in _anim.parameters) {
      _anim.SetBool(parameter.name, false);
    }

    _anim.SetBool("IsActive", true);

    Rigidbody2D rb = animator.GetComponent<Rigidbody2D>();
    rb.velocity = Vector2.zero;

    _eStateManager.StartIdle();
    
    if (_subscribers == 0) {
      _eStateManager.OnIdleEnd += HandleIdleEnd;
      _subscribers++;
    }
  }

  private void HandleIdleEnd(object sender, EventArgs e) {
    if (_eBehaviour.IsInMeleeRange()) {
      ChooseCloseRangeAttack();
    } else {
      ChooseLongRangeAttack();
    }
  }

  private void ChooseLongRangeAttack() {
    int attackRoll = Random.Range(0, 100);

    if (attackRoll < 33) {
      _anim.SetBool("IsDashing", true);
    } else if (attackRoll >= 33 && attackRoll < 66) {
      _anim.SetBool("IsThrowing", true);
    } else {
      ChooseAnyRangeAttack();
    }
  }

  private void ChooseCloseRangeAttack() {
    int attackRoll = Random.Range(0, 100);

    if (attackRoll < 50) {
      _anim.SetBool("IsSlashing", true);
    } else {
      ChooseAnyRangeAttack();
    }
  }

  private void ChooseAnyRangeAttack() {
    int attackRoll = Random.Range(0, 100);

    if (attackRoll < 50) {
      _anim.SetBool("IsDisappearing", true);
    } else {
      _anim.SetBool("IsSpinning", true);
    }
  }

  private void OnDestroy() {
    if (_subscribers > 0) {
      _eStateManager.OnIdleEnd -= HandleIdleEnd;
      _subscribers = 0;
    }
  }
}
