using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingDummyBehavior : EnemyBehaviour
{
    protected override void Awake() {
        TrainingDummyHealth trainingDummyHealth = gameObject.GetComponent<TrainingDummyHealth>();
        trainingDummyHealth.OnTrainingDummyStatusDamage += HandleStatusDamage;

        EnemyBehaviour enemyBehaviour = gameObject.GetComponent<EnemyBehaviour>();
        enemyBehaviour.OnElectrocuted += HandleElectrocuted;

        Health = trainingDummyHealth;
        base.Awake();
    }

    private void HandleElectrocuted(object sender, EventArgs e) {
        StartCoroutine(Electrocute());
    }
    protected override IEnumerator AttackPlayer() {
        yield break;
    }
}
