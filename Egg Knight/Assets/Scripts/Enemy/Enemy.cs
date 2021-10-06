using System.Collections;
using System.Collections.Generic;
using Stage;
using UnityEditor;
using UnityEngine;

public class Enemy : MonoBehaviour {
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private StageManager stageManager;
    
    protected int CoinDropRate = 20;
    private const float SurvivalDropRateMultiplier = 0.4f;
    private const float EasyDropRateMultiplier = 0.75f;
    private const float HardDropRateMultiplier = 1.2f;

    // Start is called before the first frame update
    void Awake() {
        levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected void Die() {
        stageManager = levelManager.GetCurrentStage();
        stageManager.RemoveEnemy(this);
        
        StageType stageType = stageManager.GetStageType();

        CoinDropRate = stageType switch {
            StageType.Easy => Mathf.RoundToInt(CoinDropRate * EasyDropRateMultiplier),
            StageType.Hard => Mathf.RoundToInt(CoinDropRate * HardDropRateMultiplier),
            StageType.Survival => Mathf.RoundToInt(CoinDropRate * SurvivalDropRateMultiplier),
            _ => CoinDropRate
        };

        int coinDropChance = Random.Range(1, 101);
        
        if (coinDropChance < CoinDropRate) {
            Object coinPrefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Coin.prefab", typeof(GameObject));
            Vector3 oldPos = transform.position;
            Vector3 newPos = new Vector3(oldPos.x, oldPos.y, ZcoordinateConsts.Pickup);
            Instantiate(coinPrefab, newPos, Quaternion.identity);
        }
        
        Destroy(gameObject);
    }
}
