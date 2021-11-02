using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class BroccoliIdle : StateMachineBehaviour {
  private BroccoliBehaviour _bBehaviour;
  private BroccoliStateManager _bStateManager;

  private Animator _anim;

  private static int _subscribers = 0;
  private static BroccoliMeleeAttack _lastUsedAttack = BroccoliMeleeAttack.Walk;

  public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    _bBehaviour = animator.GetComponent<EnemyBehaviour>() as BroccoliBehaviour;
    _bStateManager = animator.GetComponent<BroccoliStateManager>();

    _anim = animator;

    foreach(AnimatorControllerParameter parameter in _anim.parameters) {
      _anim.SetBool(parameter.name, false);
    }

    Rigidbody2D rb = animator.GetComponent<Rigidbody2D>();
    rb.velocity = Vector2.zero;

    _bStateManager.StartIdle();
    
    if (_subscribers == 0) {
      _bStateManager.OnIdleEnd += HandleIdleEnd;
      _subscribers++;
    }
  }

  private void HandleIdleEnd(object sender, EventArgs e) {
    if (_bBehaviour.IsInMeleeRange()) {
      ChooseCloseRangeAttack();
    } else {
      ChooseLongRangeAttack();
    }
  }

  private void ChooseCloseRangeAttack() {
    BroccoliMeleeAttack currentAttack = _lastUsedAttack;

    while (currentAttack == _lastUsedAttack) {
      float attackRoll = Random.Range(0f, 1f);

      if (attackRoll < 0.33f) {
        currentAttack = BroccoliMeleeAttack.Walk;
      } else if (attackRoll >= 0.33f && attackRoll < 0.66f) {
        currentAttack = BroccoliMeleeAttack.Slash;
      } else {
        currentAttack = BroccoliMeleeAttack.Parry;
      }
    }

    switch (currentAttack) {
      case BroccoliMeleeAttack.Walk: {
        _anim.SetBool("IsWalking", true);
        break;
      }
      case BroccoliMeleeAttack.Slash: {
        _anim.SetBool("IsChargingSlash", true);
        break;
      }
      case BroccoliMeleeAttack.Parry: {
        _anim.SetBool("IsParrying", true);
        break;
      }
      default: {
        Debug.LogError("Unkown melee attack");
        break;
      }
    }

    _lastUsedAttack = currentAttack;
  }

  private void ChooseLongRangeAttack() {
    float attackRoll = Random.Range(0f, 1f);

    if (attackRoll < 0.5f) {
      _anim.SetBool("IsChargingSpin", true);
    } else {
      _anim.SetBool("IsChargingThrow", true);
    }
  }

  private void OnDestroy() {
    if (_subscribers > 0) {
      _bStateManager.OnIdleEnd -= HandleIdleEnd;
      _subscribers = 0;
    }

    _lastUsedAttack = BroccoliMeleeAttack.Walk;
  }
}
