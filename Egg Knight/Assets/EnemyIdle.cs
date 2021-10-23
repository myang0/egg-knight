using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdle : StateMachineBehaviour {
    private EnemyBehaviour _eBehavior;
    private List<EnemyBehaviour> _otherEnemyBehaviours = new List<EnemyBehaviour>();
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        _eBehavior = animator.GetComponent<EnemyBehaviour>();
        GameObject[] enemyObjs = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var eObj in enemyObjs) {
            if (eObj != _eBehavior.gameObject) {
                _otherEnemyBehaviours.Add(eObj.GetComponent<EnemyBehaviour>());
            }
        }
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        EnemyHealth eHealth = animator.GetComponent<EnemyHealth>();
        if (_eBehavior.GetIsInAlertRange() || eHealth.GetIsHealthDamaged() || AreNearbyEnemiesAlerted()){
            animator.SetBool("isAlert", true);
        }
    }

    private bool AreNearbyEnemiesAlerted() {
        foreach (var oBehavior in _otherEnemyBehaviours) {
            if (Vector2.Distance(_eBehavior.transform.position, oBehavior.transform.position) < _eBehavior.alertRange) {
                if (oBehavior.GetComponent<Animator>().GetBool("isAlert")) {
                    return true;
                }
            }
        }
        return false;
    }
}
