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
    
    // private int _coinDropRate = 35;
    private int _coinDropRate = 50;

    private void Awake() {
        levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
    }

    public void DropCoin(Vector3 enemyPos) {
        stageManager = levelManager.GetCurrentStage();

        PlayerWallet wallet = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerWallet>();
        
        int coinDropChance = Random.Range(1, 101);
        if (wallet.GetBalance() > 30) coinDropChance += Random.Range(1, wallet.GetBalance());
        if (coinDropChance < _coinDropRate) {
            Vector3 newPos = new Vector3(enemyPos.x, enemyPos.y, ZcoordinateConsts.Character);
            Instantiate(coin, newPos, Quaternion.identity);
        }
    }
}
