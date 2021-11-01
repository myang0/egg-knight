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
    private const float EasyDropRateMultiplier = 0.8f;
    private const float HardDropRateMultiplier = 1.5f;

    private void Awake() {
        levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();

        GoldChainNecklace.OnPickup += HandleDropRateChange;
    }

    private void HandleDropRateChange(object sender, CoinRateChangeEventArgs e) {
        _coinDropRate += (int)e.rate;
    }
    
    public void DropCoin(Vector3 enemyPos) {
        stageManager = levelManager.GetCurrentStage();
        
        StageType stageType = stageManager.GetStageType();

        int tempDropRate;
        switch (stageType) {
            case StageType.Easy:
                tempDropRate = Mathf.RoundToInt(_coinDropRate * EasyDropRateMultiplier);
                break;
            case StageType.Hard:
                tempDropRate = Mathf.RoundToInt(_coinDropRate * HardDropRateMultiplier);
                break;
            default:
                tempDropRate = _coinDropRate;
                break;
        }

        int coinDropChance = Random.Range(1, 101);
        if (coinDropChance < tempDropRate) {
            Vector3 newPos = new Vector3(enemyPos.x, enemyPos.y, ZcoordinateConsts.Character);
            Instantiate(coin, newPos, Quaternion.identity);
        }
    }
}
