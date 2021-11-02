using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : StateMachineBehaviour {
    private SpawnParachute _spawnParachute;
    private SpriteRenderer _sr;
    private bool _isEnemySpawnedYet;
    private bool _isParticlesPoofedYet;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animator.updateMode = AnimatorUpdateMode.AnimatePhysics;

        _spawnParachute = animator.GetComponent<SpawnParachute>();
        _sr = animator.GetComponent<SpriteRenderer>();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (_sr.color.a > 0) {
            var color = _sr.color;
            float newAlpha;
            if (color.a > 0.64) {
                newAlpha = color.a - 0.005f;
            }
            else {
                newAlpha = color.a - 0.01f;
            }
            _sr.color = new Color(color.r, color.g, color.b, newAlpha);

            if (newAlpha < 0.8f && !_isParticlesPoofedYet) {
                _spawnParachute.GetComponent<ParticleSystem>().Play();
                _isParticlesPoofedYet = true;
            }
            
            if (newAlpha < 0.65f && !_isEnemySpawnedYet) {
                _spawnParachute.SpawnEnemy();
                _isEnemySpawnedYet = true;
            }
        }
        else {
            Destroy(animator.gameObject);
        }
    }
}
