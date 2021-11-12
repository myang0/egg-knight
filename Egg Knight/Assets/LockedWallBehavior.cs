using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedWallBehavior : EnemyBehaviour
{
    public bool isInvulnerable;
    public Sprite unlockedSprite;
    public Sprite lockedSprite;
    public SpriteRenderer sr;
    protected override void Awake() {
        LockedWallHealth lockedWallHealth = gameObject.GetComponent<LockedWallHealth>();
        lockedWallHealth.OnLockedWallStatusDamage += HandleStatusDamage;

        EnemyBehaviour enemyBehaviour = gameObject.GetComponent<EnemyBehaviour>();
        enemyBehaviour.OnElectrocuted += HandleElectrocuted;

        Health = lockedWallHealth;
        
        var position = transform.position;
        transform.position = new Vector3(position.x, position.y, ZcoordinateConsts.Object);

        Health.OnPreDeath += (sender, args) => {
            StartCoroutine(FadeOutDeath());
            UpdatePathing();
        };
        
        isTurningEnabled = false;
        SetInvulnerability(isInvulnerable);

        base.Awake();
    }

    private void HandleElectrocuted(object sender, EventArgs e) {
        StartCoroutine(Electrocute());
    }
    protected override IEnumerator AttackPlayer() {
        yield break;
    }

    public void SetInvulnerability(bool invulnerable) {
        if (invulnerable) {
            isInvulnerable = true;
            Health.isInvulnerable = true;
            sr.sprite = lockedSprite;
        }
        else {
            isInvulnerable = false;
            Health.isInvulnerable = false;
            sr.sprite = unlockedSprite;
        }
    }
    
    private void UpdatePathing() {
        var graph = AstarPath.active.data.gridGraph;
        AstarPath.active.Scan();
    }
}
