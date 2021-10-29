using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

namespace Stage {
    public class StageManager : MonoBehaviour {
        [SerializeField] private StageType stageType;
        [SerializeField] private StageStatus stageStatus;
        [SerializeField] private List<EnemyBehaviour> enemiesList;
        [SerializeField] private int numEnemiesMax;
        [SerializeField] private int numWavesMax;
        [SerializeField] private int numWavesCurr;
        [SerializeField] private StageItemStatus itemStatus = StageItemStatus.NeverSpawned;
        public event EventHandler OnStageClear;
        public event EventHandler OnStageStart;

        private StageEntrance _stageEntrance;
        private ItemSpawnpoint _itemSpawnpoint;
        private List<StageExit> _stageExits = new List<StageExit>();
        private List<EnemySpawnpoint> _eSpawnpoints = new List<EnemySpawnpoint>();
        
        private LevelManager _levelManager;
        private BoxCollider _camBoundary;
        private bool _isStageInitialized;

        // 100% - _% = Actual Spawn Rate
        private const int MediumStageSpawnRate = 98;
        private const int HardStageSpawnRate = 98;
        private const int EasyStageSpawnRate = 98;
        private const int SurvivalStageSpawnRate = 0;

        private const int NumStagesToBossLv1 = 10;
        private const int NumStagesToBossLv2 = 10;
        private const int NumStagesToBossLv3 = 12;

        private static readonly int[] Level1ItemStages = {3, 7, 11};
        private static readonly int[] Level2ItemStages = {3, 7, 11};
        private static readonly int[] Level3ItemStages = {3, 7, 11};

        private const float SurvivalTimer = 30f;
        private int _survivalTimerCurrent;
        private WaveCounterText _waveCounterText;

        private void Start() {
            _eSpawnpoints.AddRange(GetComponentsInChildren<EnemySpawnpoint>());
            _stageExits.AddRange(GetComponentsInChildren<StageExit>());
            _stageEntrance = GetComponentInChildren<StageEntrance>();
            _itemSpawnpoint = GetComponentInChildren<ItemSpawnpoint>();
            _camBoundary = GetComponent<BoxCollider>();
            _levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
            
            Assert.IsTrue(_stageExits.Count > 0);
            Assert.IsNotNull(_stageEntrance);
            Assert.IsNotNull(_camBoundary);
            Assert.IsNotNull(_levelManager);
            
            _waveCounterText = FindObjectOfType<WaveCounterText>();
        }

        private void Update() {
            if (stageStatus == StageStatus.Active) {
                if (stageType == StageType.Easy || stageType == StageType.Medium ||
                    stageType == StageType.Hard || stageType == StageType.Survival) {
                    InitializeStage();
                    SpawnEnemies();
                    
                }
                
                if (IsStageCleared()) {
                    _waveCounterText.SetText("", 0);
                    if (itemStatus == StageItemStatus.NeverSpawned) {
                        SpawnItem();
                        OnStageClear?.Invoke(this, EventArgs.Empty);
                    } else if (itemStatus == StageItemStatus.FailedSpawn ||
                              itemStatus == StageItemStatus.Collected) {
                        stageStatus = StageStatus.Cleared;
                        GenerateExits();
                    }
                }
                
            } else if (stageStatus == StageStatus.Cleared) {
                ReadyForNextStage();
            }
        }

        private void SpawnItem() {
            if (IsItemSpawning() && itemStatus == StageItemStatus.NeverSpawned) {
                ItemManager itemManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<ItemManager>();
                BaseItem baseItem = itemManager.SpawnItem(_itemSpawnpoint.transform.position);
                baseItem.OnPickup += RemoveItemFromStage;
                itemStatus = StageItemStatus.Spawned;
            }
            else {
                itemStatus = StageItemStatus.FailedSpawn;
            }
        }
        
        private void RemoveItemFromStage(object sender, EventArgs e) {
            itemStatus = StageItemStatus.Collected;
        }
        
        private bool IsStageCleared() {
            switch (stageType) {
                case StageType.Rest:
                case StageType.Shop:
                case StageType.Sirracha:
                case StageType.Spawn:   
                    return true;
                case StageType.Medium:
                case StageType.Hard:
                case StageType.Easy:
                case StageType.Boss:
                    return enemiesList.Count == 0 && numWavesCurr == numWavesMax;
                case StageType.Survival:
                    return numWavesCurr == 1 && numWavesMax == 1;
            }

            return false;
        }

        private void InitializeStage() {
            if (_isStageInitialized) return;

            switch (stageType) {
                case StageType.Hard:
                    numEnemiesMax = Mathf.RoundToInt(numEnemiesMax * 1.5f);
                    break;
                case StageType.Easy:
                    numEnemiesMax = Mathf.RoundToInt(numEnemiesMax * 0.65f);
                    break;
                case StageType.Survival:
                    numEnemiesMax = Mathf.RoundToInt(numEnemiesMax * 0.5f);
                    break;
            }
            
            //todo: this will call pizza delivery 
            OnStageStart?.Invoke(this, EventArgs.Empty);
            
            _isStageInitialized = true;
        }

        private void SpawnEnemies() {
            if (stageType == StageType.Survival) {
                if (numWavesCurr == 1 && numWavesMax == 1 || enemiesList.Count > numEnemiesMax) return;
                
                int randomSpawnIndex = Random.Range(1, _eSpawnpoints.Count);
                EnemySpawnpoint spawn = _eSpawnpoints[randomSpawnIndex];
                enemiesList.Add(spawn.SpawnEnemy(_levelManager.GetLevel()));

                if (numWavesMax != 1) {
                    StartCoroutine(StartSurvivalTimer());
                    numWavesMax = 1;
                }

            } else if (stageType == StageType.Easy ||
                       stageType == StageType.Medium ||
                       stageType == StageType.Hard) {
                
                // If all stage has enemy all at once (no waves)
                if (numWavesMax == 0) {
                    for (int i = 0; i < numEnemiesMax; i++) {
                        if (_eSpawnpoints.Count == 0) {
                            throw new Exception("Attempting to spawn " + (i+1) + "th enemy, but no more spawnpoints available.");
                        }
                        
                        int randomSpawnIndex = Random.Range(1, _eSpawnpoints.Count);
                        EnemySpawnpoint spawn = _eSpawnpoints[randomSpawnIndex];
                        enemiesList.Add(spawn.SpawnEnemy(_levelManager.GetLevel()));
                        
                        // Remove used spawnpoint to prevent enemies spawning in same spot
                        _eSpawnpoints.Remove(spawn);
                    }
                    
                    numWavesCurr++;
                    numWavesMax++;
                }
                // If stage has enemies in waves
                else {
                    if (enemiesList.Count == 0 && numWavesCurr != numWavesMax) {
                        _waveCounterText.SetText("Wave: " + (numWavesCurr+1) + "/" + numWavesMax, 0);
                        for (int i = 0; i < numEnemiesMax; i++) {
                            if (_eSpawnpoints.Count == 0) {
                                throw new Exception("Attempting to spawn " + (i+1) + "th enemy, but no more spawnpoints available.");
                            }
                            
                            int randomSpawnIndex = Random.Range(1, _eSpawnpoints.Count);
                            EnemySpawnpoint spawn = _eSpawnpoints[randomSpawnIndex];
                            enemiesList.Add(spawn.SpawnEnemy(_levelManager.GetLevel()));
                            
                            // Remove used spawnpoint to prevent enemies spawning in same spot
                            _eSpawnpoints.Remove(spawn);
                        }
                        
                        // Clear spawnpoint list and re-add to it for next wave
                        _eSpawnpoints.Clear();
                        _eSpawnpoints.AddRange(GetComponentsInChildren<EnemySpawnpoint>());
                        numWavesCurr++;
                    }
                }
            }
        }

        private void ReadyForNextStage() {
            foreach (StageExit exit in _stageExits) {
                if (exit.GetIsExitInUse()) {
                    _levelManager.NextStage(exit.GetStageType());
                    stageStatus = StageStatus.Inactive;
                    return;
                }
            }
        }

        private void GenerateExits() {
            int currentLevel = _levelManager.GetLevel();
            int stagesCleared = _levelManager.GetStagesCleared();

            if (stageType == StageType.Boss) {
                foreach (StageExit exit in _stageExits) {
                    exit.SetStageType(StageType.Spawn);
                }
            }
            // Next stage is guaranteed to be boss
            else if (currentLevel == 1 && stagesCleared == NumStagesToBossLv1 
                || currentLevel == 2 && stagesCleared == NumStagesToBossLv2
                || currentLevel == 3 && stagesCleared == NumStagesToBossLv3) {
                foreach (StageExit exit in _stageExits) {
                    exit.SetStageType(StageType.Boss);
                }
            }
            // Not enough stages cleared for boss stage
            else {
                // Flags to ensure no duplicate stages or >1 special stage spawn
                bool didSpecialRoomSpawn = false;
                bool didEasyStageSpawn = false;
                bool didMediumStageSpawn = false;
                bool didHardStageSpawn = false;
                bool didSurvivalStageSpawn = false;

                if (stageType == StageType.Shop || stageType == StageType.Rest || stageType == StageType.Sirracha) {
                    didSpecialRoomSpawn = true;
                }

                foreach (StageExit exit in _stageExits) {
                    bool skipRegularRoomGen = false;
                    
                    // Spawn stages will ALWAYS lead to a medium stage
                    if (stageType == StageType.Spawn) {
                        exit.SetStageType(StageType.Medium);
                        skipRegularRoomGen = true;
                    }
                    // Check special stage spawns
                    else if (!didSpecialRoomSpawn) {
                        if (_levelManager.GetShopSpawn()) {
                            exit.SetStageType(StageType.Shop);
                            didSpecialRoomSpawn = true;
                            skipRegularRoomGen = true;
                        }
                        else if (_levelManager.GetRestSpawn()) {
                            exit.SetStageType(StageType.Rest);
                            didSpecialRoomSpawn = true;
                            skipRegularRoomGen = true;
                        }
                        else if (_levelManager.GetSirrachaSpawn()) {
                            exit.SetStageType(StageType.Sirracha);
                            didSpecialRoomSpawn = true;
                            skipRegularRoomGen = true;
                        }
                    }
                    // Check regular stage spawns
                    if (!skipRegularRoomGen) {
                        
                        int randomStageVal;

                        attemptGeneration:
                        randomStageVal = Random.Range(1, 101);
                        if (randomStageVal > MediumStageSpawnRate && !didMediumStageSpawn) {
                            exit.SetStageType(StageType.Medium);
                            didMediumStageSpawn = true;
                        }
                        else if (randomStageVal > HardStageSpawnRate && !didHardStageSpawn) {
                            exit.SetStageType(StageType.Hard);
                            didHardStageSpawn = true;
                        }
                        else if (randomStageVal > EasyStageSpawnRate && !didEasyStageSpawn) {
                            exit.SetStageType(StageType.Easy);
                            didEasyStageSpawn = true;
                        }
                        else if (randomStageVal > SurvivalStageSpawnRate && !didSurvivalStageSpawn) {
                            exit.SetStageType(StageType.Survival);
                            didSurvivalStageSpawn = true;
                        } else {
                            // GG stage generation failed lets go again
                            goto attemptGeneration;  
                        }
                    }
                }
            }
        }

        private bool IsItemSpawning() {
            int stagesCleared = _levelManager.GetStagesCleared();

            switch (_levelManager.GetLevel()) {
                case 1 when stagesCleared == Level1ItemStages[0] ||
                            stagesCleared == Level1ItemStages[1] ||
                            stagesCleared == Level1ItemStages[2]:
                case 2 when stagesCleared == Level2ItemStages[0] ||
                            stagesCleared == Level2ItemStages[1] ||
                            stagesCleared == Level2ItemStages[2]:
                case 3 when stagesCleared == Level3ItemStages[0] ||
                            stagesCleared == Level3ItemStages[1] ||
                            stagesCleared == Level3ItemStages[2]:
                    return true;
                default:
                    return _levelManager.GetLuckyItemSpawn();
            }
        }

        private IEnumerator StartSurvivalTimer() {
            _survivalTimerCurrent = (int) SurvivalTimer;
            while (_survivalTimerCurrent > 0) {
                _waveCounterText.SetText("Survive for " + _survivalTimerCurrent + "s", 0);
                yield return new WaitForSeconds(1f);
                _survivalTimerCurrent -= 1;
            }
            foreach (EnemyBehaviour e in enemiesList) {
                Destroy(e.gameObject);
            }
            enemiesList.Clear();
            numWavesCurr = 1;
        }
        
        public void RemoveEnemy(EnemyBehaviour e) {
            enemiesList.Remove(e);
        }

        public void SetStageType(StageType newStageType) {
            this.stageType = newStageType;
        }

        public StageType GetStageType() {
            return stageType;
        }

        public void SetStageStatus(StageStatus status) {
            stageStatus = status;
        }
        
        public StageStatus GetStageStatus() {
            return stageStatus;
        }

        public BoxCollider GetCameraBoundary() {
            return _camBoundary;
        }

        public Vector3 GetSpawnPosition() {
            return _stageEntrance.GetPosition();
        }
    }
}