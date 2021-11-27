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
    
    private int _coinDropRate = 35;
    private int _moddedCoinDropRate = 35;
    private const float HardDropRateMultiplier = 1.25f;

    private void Awake() {
        levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();

        GoldChainNecklace.OnPickup += HandleDropRateChange;
    }

    private void HandleDropRateChange(object sender, CoinRateChangeEventArgs e) {
        _moddedCoinDropRate += (int)e.rate;
    }
    
    public void DropCoin(Vector3 enemyPos, bool allowDropMods) {
        stageManager = levelManager.GetCurrentStage();
        
        StageType stageType = stageManager.GetStageType();

        int tempDropRate = _coinDropRate;
        if (allowDropMods) tempDropRate = _moddedCoinDropRate;
        if (stageType == StageType.Hard) tempDropRate = Mathf.RoundToInt(_coinDropRate * HardDropRateMultiplier);

        int coinDropChance = Random.Range(1, 101);
        if (coinDropChance < tempDropRate) {
            Vector3 newPos = new Vector3(enemyPos.x, enemyPos.y, ZcoordinateConsts.Character);
            Instantiate(coin, newPos, Quaternion.identity);
        }
    }
}
