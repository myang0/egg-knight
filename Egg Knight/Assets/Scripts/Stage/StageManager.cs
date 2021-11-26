using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Pathfinding;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

namespace Stage {
    public class StageManager : MonoBehaviour {
        [SerializeField] private StageType stageType;
        [SerializeField] private StageStatus stageStatus;
        [SerializeField] public List<EnemyBehaviour> enemiesList;
        //Use enemyCount instead of enemiesList.Count because enemy spawns are delayed
        public int enemyCount;
        [SerializeField] private int numEnemiesMax;
        [SerializeField] public int numWavesMax;
        [SerializeField] public int numWavesCurr;
        [SerializeField] public StageItemStatus itemStatus = StageItemStatus.NeverSpawned;
        public bool isAutoAggroOnSpawn;
        public KeyStageStatus keyStatus;
        public List<LockedWallBehavior> lockedWalls = new List<LockedWallBehavior>();
        public event EventHandler OnStageClear;
        public event EventHandler OnStageStart;

        private StageEntrance _stageEntrance;
        private ItemSpawnpoint _itemSpawnpoint;
        public List<StageExit> stageExits = new List<StageExit>();
        public List<ChestBehavior> chests = new List<ChestBehavior>();
        private List<EnemySpawnpoint> _eSpawnpoints = new List<EnemySpawnpoint>();
        
        private LevelManager _levelManager;
        public BoxCollider _camBoundary;
        private bool _isStageInitialized;

        // 100% - _% = Actual Spawn Rate
        private const int MediumStageSpawnRate = 40;
        private const int HardStageSpawnRate = 20;
        private const int EasyStageSpawnRate = 10;
        private const int SurvivalStageSpawnRate = 0;

        private const int NumStagesToBossLv1 = 10;
        private const int NumStagesToBossLv2 = 8;
        private const int NumStagesToBossLv3 = 9;

        //Actual stage num is value+1
        private static readonly int[] Level1ItemStages = {2, 6, 10};
        private static readonly int[] Level2ItemStages = {2, 5, 8};
        private static readonly int[] Level3ItemStages = {2, 6};

        private const float SurvivalTimer = 30f;
        private int _survivalTimerCurrent;
        private WaveCounterText _waveCounterText;

        private float _hardEnemyMultiplier = 1.5f;
        private float _easyEnemyMultiplier = 0.7f;
        private float _survivalEnemyMultiplier = 0.7f;

        private bool _initialSurvivalEnemiesSpawned;

        public enum KeyStageStatus {
            NotKeyStage, KeyNotFound, KeyFound
        }

        private void Awake() {
            _eSpawnpoints.AddRange(GetComponentsInChildren<EnemySpawnpoint>());
            stageExits.AddRange(GetComponentsInChildren<StageExit>());
            lockedWalls.AddRange(GetComponentsInChildren<LockedWallBehavior>());
            chests.AddRange(GetComponentsInChildren<ChestBehavior>());
            _stageEntrance = GetComponentInChildren<StageEntrance>();
            _itemSpawnpoint = GetComponentInChildren<ItemSpawnpoint>();
            _camBoundary = GetComponent<BoxCollider>();
            _levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
            
            // Assert.IsTrue(stageExits.Count > 0)
            Assert.IsNotNull(GetComponentInChildren<ItemSpawnpoint>());
            Assert.IsNotNull(_stageEntrance);
            Assert.IsNotNull(_camBoundary);
            Assert.IsNotNull(_levelManager);
            
            _waveCounterText = FindObjectOfType<WaveCounterText>();
            if (keyStatus == KeyStageStatus.KeyNotFound) InitializeChests();
        }

        private void InitializeChests() {
            bool keySet = false;
            foreach (var c in chests) {
                int randomChance = Random.Range(1, 101);
                if (!keySet && randomChance < 100 / chests.Count) {
                    keySet = true;
                    c.drop = ChestBehavior.ChestDrops.Key;
                }
                else {
                    c.drop = randomChance < 50 ? ChestBehavior.ChestDrops.Coin : ChestBehavior.ChestDrops.Heart;
                    if (!keySet && c == chests[chests.Count-1]) {
                        int randomChestIndex = Random.Range(0, chests.Count);
                        chests[randomChestIndex].drop = ChestBehavior.ChestDrops.Key;
                    }
                }
            }
        }

        private void Update() {
            if (stageStatus == StageStatus.Active) {
                if (stageType == StageType.Easy || stageType == StageType.Medium ||
                    stageType == StageType.Hard || stageType == StageType.Survival) {
                    InitializeStage();
                    SpawnEnemies();
                }

                if (IsStageCleared()) {
                    if (stageType == StageType.Survival && _levelManager.level == 3) {
                        _waveCounterText.SetText("Enemy reinforcements have stopped.", 3);
                    }
                    else {
                        _waveCounterText.SetText("", 0);
                    }
                    UnlockLockedWalls();
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

        public void KillAllEnemies() {
            if (keyStatus == KeyStageStatus.KeyNotFound) keyStatus = KeyStageStatus.KeyFound;
            
            if (GetStageType() == StageType.Rest || GetStageType() == StageType.Shop ||
                GetStageType() == StageType.Sirracha || GetStageType() == StageType.Spawn) {
                numWavesCurr = 0;
            }
            else if (numWavesMax == 0) {
                numWavesCurr = 1;
            }
            else {
                StopCoroutine(StartSurvivalTimer());
                _waveCounterText.ResetText();
                numWavesCurr = numWavesMax;
            }
            foreach (var e in enemiesList) {
                Destroy(e.gameObject);
            }
            enemiesList.Clear();
            enemyCount = 0;
        }

        private void UnlockLockedWalls() {
            foreach (var w in lockedWalls) {
                w.SetInvulnerability(false);
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
            if (keyStatus == KeyStageStatus.KeyFound) return true;
            if (keyStatus == KeyStageStatus.KeyNotFound) return false;
            if (keyStatus == KeyStageStatus.NotKeyStage) {
                switch (stageType) {
                    case StageType.Rest:
                    case StageType.Shop:
                    case StageType.Sirracha:
                    case StageType.Spawn:
                        return true;
                    case StageType.Medium:
                    case StageType.Hard:
                    case StageType.Easy:
                        return enemyCount == 0 && numWavesCurr == numWavesMax;
                    case StageType.Boss:
                        return enemiesList.Count == 0;
                    case StageType.Survival:
                        return numWavesCurr == 1 && numWavesMax == 1;
                }
            }
            return false;
        }

        private void InitializeStage() {
            if (_isStageInitialized) return;

            switch (stageType) {
                case StageType.Hard:
                    numEnemiesMax = Mathf.RoundToInt(numEnemiesMax * _hardEnemyMultiplier);
                    break;
                case StageType.Easy:
                    numEnemiesMax = Mathf.RoundToInt(numEnemiesMax * _easyEnemyMultiplier);
                    break;
                case StageType.Survival:
                    numEnemiesMax = Mathf.RoundToInt(numEnemiesMax * _survivalEnemyMultiplier);
                    break;
            }
            
            //todo: this will call pizza delivery 
            OnStageStart?.Invoke(this, EventArgs.Empty);
            
            _isStageInitialized = true;
        }

        private void SpawnEnemies() {
            if (stageType == StageType.Survival) {
                if (numWavesCurr == 1 && numWavesMax == 1 || enemyCount >= numEnemiesMax) return;

                if (!_initialSurvivalEnemiesSpawned) {
                    if (enemyCount < numEnemiesMax) {
                        RegularEnemySpawn();
                    } else if (enemyCount == numEnemiesMax) {
                        _initialSurvivalEnemiesSpawned = true;
                    }
                }
                else {
                    if (enemyCount < numEnemiesMax) {
                        StartCoroutine(DelayedEnemySpawn());
                    } 
                }

                if (numWavesMax != 1 && keyStatus == KeyStageStatus.NotKeyStage) {
                    StartCoroutine(StartSurvivalTimer());
                    numWavesMax = 1;
                } else if (numWavesMax != 1 && keyStatus == KeyStageStatus.KeyNotFound) {
                    _waveCounterText.SetText("Warning! Enemy reinforcements will constantly arrive!", 0);
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
                        
                        _eSpawnpoints.Remove(RegularEnemySpawn());
                    }
                    
                    numWavesCurr++;
                    numWavesMax++;
                }
                // If stage has enemies in waves
                else {
                    if (enemyCount == 0 && numWavesCurr != numWavesMax) {
                        _waveCounterText.SetText("Wave: " + (numWavesCurr+1) + "/" + numWavesMax, 0);
                        for (int i = 0; i < numEnemiesMax; i++) {
                            if (_eSpawnpoints.Count == 0) {
                                throw new Exception("Attempting to spawn " + (i+1) + "th enemy, but no more spawnpoints available.");
                            }
                            _eSpawnpoints.Remove(RegularEnemySpawn());
                        }
                        
                        // Clear spawnpoint list and re-add to it for next wave
                        _eSpawnpoints.Clear();
                        _eSpawnpoints.AddRange(GetComponentsInChildren<EnemySpawnpoint>());
                        numWavesCurr++;
                    }
                }
            }
        }

        private EnemySpawnpoint RegularEnemySpawn() {
            int randomSpawnIndex = Random.Range(1, _eSpawnpoints.Count);
            EnemySpawnpoint spawn = _eSpawnpoints[randomSpawnIndex];
            spawn.SpawnEnemy();
            enemyCount++;
            return spawn;
        }

        private IEnumerator DelayedEnemySpawn() {
            enemyCount++;
            yield return new WaitForSeconds(3f);
            int randomSpawnIndex = Random.Range(1, _eSpawnpoints.Count);
            EnemySpawnpoint spawn = _eSpawnpoints[randomSpawnIndex];
            spawn.SpawnEnemy();
        }

        private void ReadyForNextStage() {
            foreach (StageExit exit in stageExits) {
                if (exit.GetIsExitInUse()) {
                    _levelManager.NextStage(exit.GetStageType());
                    KillAllEnemies();
                    stageStatus = StageStatus.Inactive;
                    return;
                }
            }
        }

        private void GenerateExits() {
            int currentLevel = _levelManager.GetLevel();
            int stagesCleared = _levelManager.GetStagesCleared();

            if (stageType == StageType.Boss) {
                foreach (StageExit exit in stageExits) {
                    exit.SetStageType(StageType.Spawn);
                }
            }
            // Next stage is guaranteed to be boss
            else if (currentLevel == 1 && stagesCleared == NumStagesToBossLv1 
                || currentLevel == 2 && stagesCleared == NumStagesToBossLv2
                || currentLevel == 3 && stagesCleared == NumStagesToBossLv3) {
                foreach (StageExit exit in stageExits) {
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

                foreach (StageExit exit in stageExits) {
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
                            stagesCleared == Level3ItemStages[1]:
                    return true;
                default:
                    return _levelManager.GetLuckyItemSpawn();
            }
        }

        public IEnumerator StartSurvivalTimer() {
            _survivalTimerCurrent = (int) SurvivalTimer;
            while (_survivalTimerCurrent > 0) {
                _waveCounterText.SetText("Survive for " + _survivalTimerCurrent + "s!", 0);
                yield return new WaitForSeconds(1f);
                _survivalTimerCurrent -= 1;
            }
            foreach (EnemyBehaviour e in enemiesList) {
                StartCoroutine(e.FadeOutDeath());
            }
            yield return new WaitForSeconds(1.5f);
            foreach (EnemyBehaviour e in enemiesList) {
                Destroy(e.gameObject);
            }
            enemiesList.Clear();
            enemyCount = 0;
            numWavesCurr = 1;
        }
        
        public void RemoveEnemy(EnemyBehaviour e) {
            enemiesList.Remove(e);
            if (e.decrementEnemyCountOnDeath) enemyCount--;
        }

        public void AddEnemy(EnemyBehaviour e) {
            enemiesList.Add(e);
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

        public List<EnemySpawnpoint> GetEnemySpawnPoints() {
            return _eSpawnpoints;
        }
    }
}