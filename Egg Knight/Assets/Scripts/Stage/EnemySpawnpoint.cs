using System;
using System.Collections;
using System.Collections.Generic;
using Stage;
using UnityEngine;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;

public class EnemySpawnpoint : MonoBehaviour {
    // [SerializeField] private Enemy placeholderEnemy;
    [SerializeField] private EnemyBehaviour lv1EggGuard;
    [SerializeField] private EnemyBehaviour lv1Mushroom;
    [SerializeField] private EnemyBehaviour lv1Raspberry;
    [SerializeField] private EnemyBehaviour lv1Strawberry;

    // Spawn Rates
    private const int Lv1EggGuardRate = 70;
    private const int Lv1MushroomRate = 45;
    private const int Lv1RaspberryRate = 20;
    private const int Lv1StrawberryRate = 0;

    // Start is called before the first frame update
    void Start() {
        StartAsserts();
    }

    private void StartAsserts() {
        Assert.IsNotNull(lv1EggGuard);
        // Assert.IsNotNull(lv1Mushroom);
        Assert.IsNotNull(lv1Raspberry);
        // Assert.IsNotNull(lv1Strawberry);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public EnemyBehaviour SpawnEnemy(int level) {
        switch (level) {
            case 1:
                // return SpawnEnemy(placeholderEnemy);
                return SpawnLevel1();   
            case 2:
                // return SpawnEnemy(placeholderEnemy);
                return SpawnLevel1();   
                //return SpawnLevel2();
            case 3:
                // return SpawnEnemy(placeholderEnemy);
                return SpawnLevel1();   
                //return SpawnLevel3();
            default:
                throw new Exception("Attempting to spawn an enemy in level >3???");
        }
    }

    private EnemyBehaviour SpawnEnemy(EnemyBehaviour enemy) {
        Debug.Log("SPAWNING");
        Vector3 oldPos = transform.position;
        Vector3 newPos = new Vector3(oldPos.x, oldPos.y, ZcoordinateConsts.Character);
        EnemyBehaviour newEnemy = Instantiate(enemy, newPos, Quaternion.identity);
        return newEnemy;
    }

    private EnemyBehaviour SpawnLevel1() {
        int enemyChance = Random.Range(1, 101);
        
        if (enemyChance > Lv1EggGuardRate) {
            return SpawnEnemy(lv1EggGuard);
        }

        if (enemyChance > Lv1MushroomRate) {
            // SpawnEnemy(lv1Mushroom);   
            return SpawnEnemy(lv1Mushroom);
        }

        if (enemyChance > Lv1RaspberryRate) {
            return SpawnEnemy(lv1Raspberry);         
        }

        if (enemyChance > Lv1StrawberryRate) {
            // SpawnEnemy(lv1Strawberry);
            return SpawnEnemy(lv1Raspberry); 
        }

        return null;
    }
    
    private EnemyBehaviour SpawnLevel2() {
        int enemyChance = Random.Range(1, 101);
        
        if (enemyChance > Lv1EggGuardRate) {
            return SpawnEnemy(null);        
        }

        if (enemyChance > Lv1MushroomRate) {
            return SpawnEnemy(null); 
        }

        if (enemyChance > Lv1RaspberryRate) {
            return SpawnEnemy(null); 
        }

        if (enemyChance > Lv1StrawberryRate) {
            return SpawnEnemy(null); 
        }

        return null;
    }
    
    private EnemyBehaviour SpawnLevel3() {
        int enemyChance = Random.Range(1, 101);
        
        if (enemyChance > Lv1EggGuardRate) {
            return SpawnEnemy(null); 
        }

        if (enemyChance > Lv1MushroomRate) {
            return SpawnEnemy(null); 
        }

        if (enemyChance > Lv1RaspberryRate) {
            return SpawnEnemy(null); 
        }

        if (enemyChance > Lv1StrawberryRate) {
            return SpawnEnemy(null); 
        }

        return null;
    }
}
