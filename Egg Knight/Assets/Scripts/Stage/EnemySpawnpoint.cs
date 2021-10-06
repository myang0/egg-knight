using System;
using System.Collections;
using System.Collections.Generic;
using Stage;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawnpoint : MonoBehaviour {
    [SerializeField] private Enemy placeholderEnemy;
    [SerializeField] private Enemy lv1EggGuard;
    [SerializeField] private Enemy lv1Mushroom;
    [SerializeField] private Enemy lv1Raspberry;
    [SerializeField] private Enemy lv1Strawberry;

    // Spawn Rates
    private const int Lv1EggGuardRate = 70;
    private const int Lv1MushroomRate = 45;
    private const int Lv1RaspberryRate = 20;
    private const int Lv1StrawberryRate = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Enemy SpawnEnemy(int level) {
        switch (level) {
            case 1:
                return SpawnEnemy(placeholderEnemy);
                //return SpawnLevel1();   
            case 2:
                return SpawnEnemy(placeholderEnemy);
                //return SpawnLevel2();
            case 3:
                return SpawnEnemy(placeholderEnemy);
                //return SpawnLevel3();
            default:
                throw new Exception("Attempting to spawn an enemy in level >3???");
        }
    }

    private Enemy SpawnEnemy(Enemy enemy) {
        Debug.Log("SPAWNING");
        Vector3 oldPos = transform.position;
        Vector3 newPos = new Vector3(oldPos.x, oldPos.y, ZcoordinateConsts.Character);
        return Instantiate(enemy, newPos, Quaternion.identity);
    }

    private Enemy SpawnLevel1() {
        int enemyChance = Random.Range(1, 101);
        
        if (enemyChance > Lv1EggGuardRate) {
            SpawnEnemy(lv1EggGuard);
        } else if (enemyChance > Lv1MushroomRate) {
            SpawnEnemy(lv1Mushroom);        
        } else if (enemyChance > Lv1RaspberryRate) {
            SpawnEnemy(lv1Raspberry);         
        } else if (enemyChance > Lv1StrawberryRate) {
            SpawnEnemy(lv1Strawberry);       
        }

        return null;
    }
    
    private Enemy SpawnLevel2() {
        int enemyChance = Random.Range(1, 101);
        
        if (enemyChance > Lv1EggGuardRate) {
            SpawnEnemy(null);        
        } else if (enemyChance > Lv1MushroomRate) {
            SpawnEnemy(null); 
        } else if (enemyChance > Lv1RaspberryRate) {
            SpawnEnemy(null); 
        } else if (enemyChance > Lv1StrawberryRate) {
            SpawnEnemy(null); 
        }

        return null;
    }
    
    private Enemy SpawnLevel3() {
        int enemyChance = Random.Range(1, 101);
        
        if (enemyChance > Lv1EggGuardRate) {
            SpawnEnemy(null); 
        } else if (enemyChance > Lv1MushroomRate) {
            SpawnEnemy(null); 
        } else if (enemyChance > Lv1RaspberryRate) {
            SpawnEnemy(null); 
        } else if (enemyChance > Lv1StrawberryRate) {
            SpawnEnemy(null); 
        }

        return null;
    }
}
