using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class EggnaIdleState : StateMachineBehaviour {
  private EggnaBehaviour _eBehaviour;
  private EggnaStateManager _eStateManager;

  private Animator _anim;

  private static int _subscribers = 0;

  private static EggnaAnyRangeAttack _lastUsedAttack = EggnaAnyRangeAttack.Spin;
  private static EggnaCloseRangeAttack _lastUsedCloseRangeAttack = EggnaCloseRangeAttack.Any;
  private static EggnaLongRangeAttack _lastUsedLongRangeAttack = EggnaLongRangeAttack.Any;

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
    } else if (_eBehaviour.IsInLongRange()) {
      ChooseLongRangeAttack();
    } else {
      ChooseAnyRangeAttack();
    }
  }

  private void ChooseLongRangeAttack() {
    EggnaLongRangeAttack currentAttack = _lastUsedLongRangeAttack;

    while (currentAttack == _lastUsedLongRangeAttack) {
      int attackRoll = Random.Range(0, 100);

      if (attackRoll < 33) {
        currentAttack = EggnaLongRangeAttack.Dash;
      } else if (attackRoll >= 33 && attackRoll < 66) {
        currentAttack = EggnaLongRangeAttack.Throw;
      } else {
        currentAttack = EggnaLongRangeAttack.Any;
      }
    }

    if (currentAttack == EggnaLongRangeAttack.Dash) {
      _anim.SetBool("IsDashing", true);
    } else if (currentAttack == EggnaLongRangeAttack.Throw) {
      _anim.SetBool("IsThrowing", true);
    } else {
      ChooseAnyRangeAttack();
    }

    _lastUsedLongRangeAttack = currentAttack;
  }

  private void ChooseCloseRangeAttack() {
    EggnaCloseRangeAttack currentAttack = _lastUsedCloseRangeAttack;

    while (currentAttack == _lastUsedCloseRangeAttack) {
      int attackRoll = Random.Range(0, 100);

      if (attackRoll < 50) {
        currentAttack = EggnaCloseRangeAttack.Slash;
      } else {
        currentAttack = EggnaCloseRangeAttack.Any;
      }
    }

    if (currentAttack == EggnaCloseRangeAttack.Slash) {
      _anim.SetBool("IsSlashing", true);
    } else {
      ChooseAnyRangeAttack();
    }

    _lastUsedCloseRangeAttack = currentAttack;
  }

  private void ChooseAnyRangeAttack() {
    EggnaAnyRangeAttack currentAttack = _lastUsedAttack;

    while (currentAttack == _lastUsedAttack) {
      int attackRoll = Random.Range(0, 100);

      if (attackRoll < 50) {
        currentAttack = EggnaAnyRangeAttack.Teleport;
      } else {
        currentAttack = EggnaAnyRangeAttack.Spin;
      }
    }

    if (currentAttack == EggnaAnyRangeAttack.Teleport) {
      _anim.SetBool("IsDisappearing", true);
    } else {
      _anim.SetBool("IsSpinning", true);
    }

    _lastUsedAttack = currentAttack;
  }

  private void OnDestroy() {
    if (_subscribers > 0) {
      _eStateManager.OnIdleEnd -= HandleIdleEnd;
      _subscribers = 0;
    }

    _lastUsedAttack = EggnaAnyRangeAttack.Spin;
    _lastUsedCloseRangeAttack = EggnaCloseRangeAttack.Any;
    _lastUsedLongRangeAttack = EggnaLongRangeAttack.Any;
  }
}
