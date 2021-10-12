using System;
using System.Collections;
using System.Collections.Generic;
using Stage;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class CoinDrop : MonoBehaviour
{
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private StageManager stageManager;
    [SerializeField] private Coin coin;
    
    private int _coinDropRate = 20;
    private const float SurvivalDropRateMultiplier = 0.4f;
    private const float EasyDropRateMultiplier = 0.75f;
    private const float HardDropRateMultiplier = 1.2f;
    private void Awake() {
        levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
    }
    
    public void DropCoin(Vector3 enemyPos) {
        stageManager = levelManager.GetCurrentStage();
        
        StageType stageType = stageManager.GetStageType();

        _coinDropRate = stageType switch {
            StageType.Easy => Mathf.RoundToInt(_coinDropRate * EasyDropRateMultiplier),
            StageType.Hard => Mathf.RoundToInt(_coinDropRate * HardDropRateMultiplier),
            StageType.Survival => Mathf.RoundToInt(_coinDropRate * SurvivalDropRateMultiplier),
            _ => _coinDropRate
        };

        int coinDropChance = Random.Range(1, 101);
        if (coinDropChance < _coinDropRate) {
            Vector3 newPos = new Vector3(enemyPos.x, enemyPos.y, ZcoordinateConsts.Pickup);
            Instantiate(coin, newPos, Quaternion.identity);
        }
    }
}
