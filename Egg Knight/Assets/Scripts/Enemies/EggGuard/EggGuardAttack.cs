using System;
using UnityEngine;

public class EggGuardAttack : StateMachineBehaviour
{
  public event EventHandler OnEggGuardAttackBegin;
  public event EventHandler OnEggGuardAttackEnd;

  override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    OnEggGuardAttackBegin?.Invoke(this, EventArgs.Empty);
  }

  override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    OnEggGuardAttackEnd?.Invoke(this, EventArgs.Empty);
  }
}
