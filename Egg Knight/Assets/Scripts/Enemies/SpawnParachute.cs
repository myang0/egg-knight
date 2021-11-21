using System;
using System.Collections;
using System.Collections.Generic;
using Stage;
using UnityEngine;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;

public class SpawnParachute : MonoBehaviour {
    public EnemyBehaviour lv1EggGuard;
    public EnemyBehaviour lv1Mushroom;
    public EnemyBehaviour lv1Raspberry;
    public EnemyBehaviour lv1Strawberry;

    public EnemyBehaviour lv2Tomato;
    public EnemyBehaviour lv2PeaPod;
    public EnemyBehaviour lv2Corn;

    public EnemyBehaviour spawnSpecificEnemy;
    
    // Spawn Rates
    private const int Lv1EggGuardRate = 70;
    private const int Lv1MushroomRate = 45;
    private const int Lv1RaspberryRate = 20;
    private const int Lv1StrawberryRate = 0;

    private const int Lv2TomatoRate = 50;
    private const int Lv2CornRate = 30;
    private const int Lv2PeaPodRate = 20;
    
    void Awake() {
        StartAsserts();
        Vector3 pos = transform.position;
        transform.position = new Vector3(pos.x, pos.y, ZcoordinateConsts.Pickup);
    }

    private void StartAsserts() {
        Assert.IsNotNull(lv1EggGuard);
        Assert.IsNotNull(lv1Mushroom);
        Assert.IsNotNull(lv1Raspberry);
        Assert.IsNotNull(lv1Strawberry);
        Assert.IsNotNull(lv2Tomato);
        Assert.IsNotNull(lv2PeaPod);
        Assert.IsNotNull(lv2Corn);
    }
    
    public void SpawnEnemy() {
        LevelManager levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
        EnemyBehaviour spawnedEnemy;

        if (spawnSpecificEnemy == null) {
            switch (levelManager.GetLevel()) {
                case 1:
                    spawnedEnemy = SpawnLevel1();
                    break;
                case 2:
                    int roll = Random.Range(0, 100);
                    if (roll < 66) {
                        spawnedEnemy = SpawnLevel1();
                    } else {
                        spawnedEnemy = SpawnLevel2();
                    }
                    
                    break;
                case 3:
                    spawnedEnemy = SpawnLevel1();
                    // spawnedEnemy = SpawnLevel3();
                    break;
                default:
                    throw new Exception("Attempting to spawn an enemy in level >3???");
            }
        }
        else {
            spawnedEnemy = InstantiateEnemy(spawnSpecificEnemy);
        }

        GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>().GetCurrentStage().AddEnemy(spawnedEnemy);
    }

    private EnemyBehaviour InstantiateEnemy(EnemyBehaviour enemy) {
        Debug.Log("SPAWNING");
        Vector3 oldPos = transform.position;
        Vector3 newPos = new Vector3(oldPos.x, oldPos.y, ZcoordinateConsts.Character);
        EnemyBehaviour newEnemy = Instantiate(enemy, newPos, Quaternion.identity);
        return newEnemy;
    }

    private EnemyBehaviour SpawnLevel1() {
        int enemyChance = Random.Range(1, 101);
        
        if (enemyChance > Lv1EggGuardRate) {
            return InstantiateEnemy(lv1EggGuard);
        }

        if (enemyChance > Lv1MushroomRate) {
            return InstantiateEnemy(lv1Mushroom);
        }

        if (enemyChance > Lv1RaspberryRate) {
            return InstantiateEnemy(lv1Raspberry);         
        }

        if (enemyChance > Lv1StrawberryRate) {
            return InstantiateEnemy(lv1Strawberry);
        }

        return null;
    }
    
    private EnemyBehaviour SpawnLevel2() {
        int enemyRoll = Random.Range(0, 100);

        if (enemyRoll < Lv2TomatoRate) {
            return InstantiateEnemy(lv2Tomato);
        } else if (enemyRoll >= Lv2TomatoRate && enemyRoll < Lv2TomatoRate + Lv2CornRate) {
            return InstantiateEnemy(lv2Corn);
        } else {
            return InstantiateEnemy(lv2PeaPod);
        }

        return null;
    }
    
    private EnemyBehaviour SpawnLevel3() {
        int enemyChance = Random.Range(1, 101);
        
        if (enemyChance > Lv1EggGuardRate) {
            return InstantiateEnemy(null); 
        }
        if (enemyChance > Lv1MushroomRate) {
            return InstantiateEnemy(null); 
        }

        if (enemyChance > Lv1RaspberryRate) {
            return InstantiateEnemy(null); 
        }

        if (enemyChance > Lv1StrawberryRate) {
            return InstantiateEnemy(null); 
        }

        return null;
    }
}
