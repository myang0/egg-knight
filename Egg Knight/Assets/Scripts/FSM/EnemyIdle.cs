using System.Collections;
using System.Collections.Generic;
using Stage;
using UnityEngine;

public class EnemyIdle : StateMachineBehaviour {
    private EnemyBehaviour _eBehavior;
    private EnemyHealth _eHealth;
    private List<EnemyBehaviour> _otherEnemyBehaviours = new List<EnemyBehaviour>();
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        _eBehavior = animator.GetComponent<EnemyBehaviour>();
        _eHealth = animator.GetComponent<EnemyHealth>();
        _eBehavior.isWallCollisionOn = true;
        SetAlertIfSurvival(animator);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (_eBehavior.isDead) animator.SetTrigger("triggerDead");
        RemoveDeadEnemiesList();
        AddNewAliveEnemiesList();
        if (_eBehavior.isStunned) animator.SetBool("isStunned", true);
        if (_eBehavior.GetIsInAlertRange() || _eHealth.GetIsHealthDamaged() || AreNearbyEnemiesAlerted()){
            animator.SetBool("isAlert", true);
            _eBehavior.SetAlertTrigger();
        }
        _eBehavior.Wander();
    }

    private void SetAlertIfSurvival(Animator animator) {
        LevelManager levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
        StageManager stageManager = levelManager.GetCurrentStage();
        if (stageManager.GetStageType() == StageType.Survival) {
            animator.SetBool("isAlert", true);
            _eBehavior.SetAlertTrigger();
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

    private void RemoveDeadEnemiesList() {
        List<EnemyBehaviour> deadEnemies = new List<EnemyBehaviour>();

        foreach (var otherEnemy in _otherEnemyBehaviours) {
            if (otherEnemy == null) {
                deadEnemies.Add(otherEnemy);
            }
        }
        
        foreach (var deadEnemy in deadEnemies) {
            _otherEnemyBehaviours.Remove(deadEnemy);
        }
    }

    private void AddNewAliveEnemiesList() {
        GameObject[] enemyObjs = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var eObj in enemyObjs) {
            if (eObj != _eBehavior.gameObject && !_otherEnemyBehaviours.Contains(eObj.GetComponent<EnemyBehaviour>())) {
                _otherEnemyBehaviours.Add(eObj.GetComponent<EnemyBehaviour>());
            }
        }
    }

    private bool AreFewEnemiesAlive() {
        return _otherEnemyBehaviours.Count < 3;
    }
}
