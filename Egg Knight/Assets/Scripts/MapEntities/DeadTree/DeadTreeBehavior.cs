using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class DeadTreeBehavior : EnemyBehaviour {
    public Transform topHalf;
    public SpriteRenderer SR;
    public SpriteRenderer topHalfSR;
    public Sprite normalTreeSprite;
    public Sprite normalTreeTopSprite;
    public Sprite deadTreeSprite;
    public Sprite deadTreeTopSprite;
    public bool isInvulnerable;
    public BoxCollider2D pathingCollider;
    protected override void Awake() {
        DeadTreeHealth deadTreeHealth = gameObject.GetComponent<DeadTreeHealth>();
        deadTreeHealth.OnDeadTreeStatusDamage += HandleStatusDamage;

        EnemyBehaviour enemyBehaviour = gameObject.GetComponent<EnemyBehaviour>();
        enemyBehaviour.OnElectrocuted += HandleElectrocuted;

        Health = deadTreeHealth;
        
        var position = transform.position;
        transform.position = new Vector3(position.x, position.y, ZcoordinateConsts.Interactable);

        position = topHalf.position;
        topHalf.position = new Vector3(position.x, position.y, ZcoordinateConsts.OverCharacter);

        Health.OnPreDeath += (sender, args) => {
            StartCoroutine(FadeOutDeathTopHalf());
        };
        
        isTurningEnabled = false;
        SetInvulnerability(isInvulnerable);

        var color = topHalfSR.color;
        topHalfSR.color = new Color(color.r, color.g, color.b, 0.8f);
        SR.color = new Color(color.r, color.g, color.b, 0.8f);
        
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
            SR.sprite = normalTreeSprite;
            topHalfSR.sprite = normalTreeTopSprite;
            isInvulnerable = true;
            Health.isInvulnerable = true;
        }
        else {
            SR.sprite = deadTreeSprite;
            topHalfSR.sprite = deadTreeTopSprite;
            isInvulnerable = false;
            Health.isInvulnerable = false;
        }
    }

    private IEnumerator FadeOutDeathTopHalf() {
        isDead = true;
        GetComponent<Collider2D>().enabled = false;
    
        Quaternion newRotation = Quaternion.Euler(0, 0, 90);
        var newPos = topHalf.position;
        topHalf.position = new Vector3(newPos.x, newPos.y, ZcoordinateConsts.Interactable);

        UpdatePathing();

        while (topHalfSR.color.a > 0) {
            var color = topHalfSR.color;
            float newAlpha = color.a -= 0.001f;
            topHalfSR.color = new Color(color.r, color.g, color.b, newAlpha);
            yield return null;
        }
    }

    private void UpdatePathing() {
        var graph = AstarPath.active.data.gridGraph;
        AstarPath.active.Scan();
    }
}
