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
    if (_eBehaviour.IsSpeaking) {
      _eStateManager.StartIdle();
      return;
    }

    if (_eBehaviour.IsInMeleeRange()) {
      ChooseCloseRangeAttack();
    } else {
      ChooseAnyRangeAttack();
    }
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

      if (attackRoll < 25) {
        currentAttack = EggnaAnyRangeAttack.Teleport;
      } else if (attackRoll >= 25 && attackRoll < 50) {
        currentAttack = EggnaAnyRangeAttack.Spin;
      } else if (attackRoll >= 50 && attackRoll < 75) {
        currentAttack = EggnaAnyRangeAttack.Throw;
      } else {
        currentAttack = EggnaAnyRangeAttack.Dash;
      }
    }

    switch (currentAttack) {
      case EggnaAnyRangeAttack.Teleport: {
        _anim.SetBool("IsDisappearing", true);
        break;
      }
      case EggnaAnyRangeAttack.Spin: {
        _anim.SetBool("IsSpinning", true);
        break;
      }
      case EggnaAnyRangeAttack.Throw: {
        _anim.SetBool("IsThrowing", true);
        break;
      }
      case EggnaAnyRangeAttack.Dash: {
        _anim.SetBool("IsDashing", true);
        break;
      }
      default: {
        Debug.LogError("Unknown attack");
        break;
      }
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
  }
}
