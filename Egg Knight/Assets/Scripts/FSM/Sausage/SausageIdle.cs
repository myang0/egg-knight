using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class SausageIdle : StateMachineBehaviour {
  private SausageBehaviour _sBehaviour;
  private SausageStateManager _sStateManager;

  private Animator _anim;

  private static int _subscribers = 0;
  private static SausageAttack _lastUsedAttack = SausageAttack.Bomb;

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
    SausageAttack currentAttack = _lastUsedAttack;

    while (currentAttack == _lastUsedAttack) {
      int attackRoll = Random.Range(0, 100);

      if (attackRoll < 25) {
        currentAttack = SausageAttack.Bomb;
      } else if (attackRoll >= 25 && attackRoll < 50) {
        currentAttack = SausageAttack.Walk;
      } else if (attackRoll >= 50 && attackRoll < 75 && SausagePartyAttack.Partygoers <= 3) {
        currentAttack = SausageAttack.Party;
      } else {
        currentAttack = SausageAttack.Spin;
      }
    }

    switch (currentAttack) {
      case SausageAttack.Bomb: {
        _anim.SetBool("IsBombing", true);
        break;
      }
      case SausageAttack.Walk: {
        _anim.SetBool("IsWalking", true);
        break;
      }
      case SausageAttack.Party: {
        _anim.SetBool("IsPartying", true);
        break;
      }
      case SausageAttack.Spin: {
        _anim.SetBool("IsChargingSpin", true);
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
      _sStateManager.OnIdleEnd -= HandleIdleEnd;
      _subscribers = 0;
    }

    Debug.Log("asdf");
    _lastUsedAttack = SausageAttack.Bomb;
  }
}
