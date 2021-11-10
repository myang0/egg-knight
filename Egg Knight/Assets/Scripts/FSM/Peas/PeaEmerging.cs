using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeaEmerging : StateMachineBehaviour
{
  private EnemyBehaviour _eBehaviour;
  private EnemyHealth _eHealth;

  override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    _eBehaviour = animator.GetComponent<EnemyBehaviour>();
    _eHealth = animator.GetComponent<EnemyHealth>();

    _eBehaviour.rb.velocity = new Vector2(Random.Range(-0.5f, 0.5f), 0);
    _eHealth.isInvulnerable = true;
  }

  override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    _eBehaviour.rb.velocity = Vector2.zero;
    _eHealth.isInvulnerable = false;
  }
}