using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWander : StateMachineBehaviour
{
    private EnemyBehaviour _eBehavior;
    private EnemyHealth _eHealth;
    private List<EnemyBehaviour> _otherEnemyBehaviours = new List<EnemyBehaviour>();
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        _eBehavior = animator.GetComponent<EnemyBehaviour>();
        _eHealth = animator.GetComponent<EnemyHealth>();
        GameObject[] enemyObjs = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var eObj in enemyObjs) {
            if (eObj != _eBehavior.gameObject) {
                _otherEnemyBehaviours.Add(eObj.GetComponent<EnemyBehaviour>());
            }
        }
        _eBehavior.Wander();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (_eBehavior.isStunned) animator.SetBool("isStunned", true);
        if (_eBehavior.GetIsInAlertRange() || _eHealth.GetIsHealthDamaged() || AreNearbyEnemiesAlerted()){
            animator.SetBool("isAlert", true);
            animator.SetBool("isWandering", false);
        }

        if (!_eBehavior.isWandering) {
            animator.SetBool("isWandering", false);
        }
    }
    
    private bool AreNearbyEnemiesAlerted() {
        foreach (var oBehavior in _otherEnemyBehaviours) {
            if (oBehavior != null) {
                if (oBehavior.GetComponent<Animator>().GetBool("isAlert") &&
                    Vector2.Distance(_eBehavior.transform.position, oBehavior.transform.position) <
                    _eBehavior.alertRange) {
                    return true;
                }
            }
        }
        return false;
    }
}
