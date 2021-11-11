using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ChestBehavior : EnemyBehaviour
{
    public bool isInvulnerable;
    public Sprite openedSprite;
    public GameObject coinObj;
    public GameObject heartObj;
    public GameObject keyObj;
    public ChestDrops drop;
    public int maxAmt;
    public int minAmt;
    
    public enum ChestDrops {
        Coin, Heart, Key
    }
    
    protected override void Awake() {
        ChestHealth chestHealth = gameObject.GetComponent<ChestHealth>();
        chestHealth.OnChestStatusDamage += HandleStatusDamage;

        EnemyBehaviour enemyBehaviour = gameObject.GetComponent<EnemyBehaviour>();
        enemyBehaviour.OnElectrocuted += HandleElectrocuted;

        Health = chestHealth;
        
        var position = transform.position;
        transform.position = new Vector3(position.x, position.y, ZcoordinateConsts.Object);

        Health.OnPreDeath += (sender, args) => {
            GetComponent<SpriteRenderer>().sprite = openedSprite;
            SpawnDrops();
            StartCoroutine(FadeOutDeath());
            UpdatePathing();
        };
        
        isTurningEnabled = false;
        SetInvulnerability(isInvulnerable);

        base.Awake();
    }

    private void SpawnDrops() {
        float maxOffset = 0.6f;
        
        if (drop == ChestDrops.Coin) {
            int randomAmt = Random.Range(minAmt, maxAmt+1);
            for (int i = 0; i < randomAmt; i++) {
                Vector3 pos = transform.position;
                Vector3 spawnPos = new Vector3(pos.x + Random.Range(-maxOffset, maxOffset), pos.y + Random.Range(-maxOffset, maxOffset), ZcoordinateConsts.Pickup);
                Instantiate(coinObj, spawnPos, Quaternion.identity);
            }
        } else if (drop == ChestDrops.Heart) {
            int randomAmt = Random.Range(minAmt, maxAmt+1);
            for (int i = 0; i < randomAmt; i++) {
                Vector3 pos = transform.position;
                Vector3 spawnPos = new Vector3(pos.x + Random.Range(-maxOffset, maxOffset), pos.y + Random.Range(-maxOffset, maxOffset), ZcoordinateConsts.Pickup);
                Instantiate(heartObj, spawnPos, Quaternion.identity);
            }
            
        } else if (drop == ChestDrops.Key) {
            Instantiate(keyObj, transform.position, Quaternion.identity);
        }
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
        }
        else {
            isInvulnerable = false;
            Health.isInvulnerable = false;
        }
    }
    
    private void UpdatePathing() {
        var graph = AstarPath.active.data.gridGraph;
        AstarPath.active.Scan();
    }
}
