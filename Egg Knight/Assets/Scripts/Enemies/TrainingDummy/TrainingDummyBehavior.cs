using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingDummyBehavior : EnemyBehaviour {
    private SpriteRenderer _sr;
    
    protected override void Awake() {
        TrainingDummyHealth trainingDummyHealth = gameObject.GetComponent<TrainingDummyHealth>();
        trainingDummyHealth.OnTrainingDummyStatusDamage += HandleStatusDamage;

        EnemyBehaviour enemyBehaviour = gameObject.GetComponent<EnemyBehaviour>();
        enemyBehaviour.OnElectrocuted += HandleElectrocuted;

        Health = trainingDummyHealth;
        _sr = GetComponent<SpriteRenderer>();
        _sr.color = new Color(255, 255, 255, 0);
        
        base.Awake();
    }

    private void HandleElectrocuted(object sender, EventArgs e) {
        StartCoroutine(Electrocute());
    }
    protected override IEnumerator AttackPlayer() {
        yield break;
    }

    public void RevealSelf() {
        GetComponent<ParticleSystem>().Play();
        StartCoroutine(IncreaseAlpha());
    }

    private IEnumerator IncreaseAlpha() {
        while (_sr.color.a < 1) {
            float newAlpha = _sr.color.a;
            newAlpha += 0.005f;
            _sr.color = new Color(255, 255, 255, newAlpha);
            yield return null;
        }
    }
}
