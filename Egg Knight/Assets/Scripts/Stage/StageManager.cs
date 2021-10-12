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
        
        private StageEntrance _stageEntrance;
        private List<StageExit> _stageExits = new List<StageExit>();
        private List<EnemySpawnpoint> _eSpawnpoints = new List<EnemySpawnpoint>();
        
        private LevelManager _levelManager;
        private BoxCollider _camBoundary;
        private bool _isDifficultyInitialized;

        // 100% - _% = Actual Spawn Rate
        private const int MediumStageSpawnRate = 50;
        private const int HardStageSpawnRate = 30;
        private const int EasyStageSpawnRate = 10;
        private const int SurvivalStageSpawnRate = 0;

        private const int NumStagesToBossLv1 = 10;
        private const int NumStagesToBossLv2 = 12;
        private const int NumStagesToBossLv3 = 15;
        
        private const float SurvivalTimer = 30f;

        private void Start() {
            _eSpawnpoints.AddRange(GetComponentsInChildren<EnemySpawnpoint>());
            _stageExits.AddRange(GetComponentsInChildren<StageExit>());
            _stageEntrance = GetComponentInChildren<StageEntrance>();
            _camBoundary = GetComponent<BoxCollider>();
            _levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
            
            Assert.IsTrue(_stageExits.Count > 0);
            Assert.IsNotNull(_stageEntrance);
            Assert.IsNotNull(_camBoundary);
            Assert.IsNotNull(_levelManager);
        }

        private void Update() {
            if (stageStatus == StageStatus.Active) {
                if (stageType == StageType.Easy || stageType == StageType.Medium ||
                    stageType == StageType.Hard || stageType == StageType.Survival) {
                    InitializeDifficulty();
                    SpawnEnemies();
                    
                }
                
                if (StageClearCondition()) {
                    stageStatus = StageStatus.Cleared;
                    ClearStage();
                    GenerateExits();
                }
                
            } else if (stageStatus == StageStatus.Cleared) {
                ReadyForNextStage();
            }
        }
        
        private bool StageClearCondition() {
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

        private void InitializeDifficulty() {
            if (_isDifficultyInitialized) return;
            
            if (stageType == StageType.Hard) {
                numEnemiesMax = Mathf.RoundToInt(numEnemiesMax * 1.5f);
            } else if (stageType == StageType.Easy) {
                numEnemiesMax = Mathf.RoundToInt(numEnemiesMax * 0.65f);
            }

            _isDifficultyInitialized = true;
        }

        private void SpawnEnemies() {
            if (stageType == StageType.Survival) {
                int survivalEnemiesMax = numEnemiesMax;
                // Reduce max enemies on survival if it is not wave based
                if (numWavesMax == 0) survivalEnemiesMax = Mathf.RoundToInt(survivalEnemiesMax * 0.8f);
                
                if (numWavesCurr == 1 && numWavesMax == 1 || enemiesList.Count >= survivalEnemiesMax) return;
                
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
        
        private void ClearStage() {
            if (stageType != StageType.Spawn) {
                _levelManager.IncrementStagesCleared();
                _levelManager.IncrementRest(5);
                _levelManager.IncrementShop(5);
                _levelManager.IncrementSirracha(5);  
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
                Debug.Log("Exits are all set to SPAWN");
                foreach (StageExit exit in _stageExits) {
                    exit.SetStageType(StageType.Spawn);
                }
            }
            // Next stage is guaranteed to be boss
            else if (currentLevel == 1 && stagesCleared == NumStagesToBossLv1 
                || currentLevel == 2 && stagesCleared == NumStagesToBossLv2
                || currentLevel == 3 && stagesCleared == NumStagesToBossLv3) {
                Debug.Log("Exits are all set to BOSS");
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
                        Debug.Log("Generated a MEDIUM exit");
                    }
                    // Check special stage spawns
                    else if (!didSpecialRoomSpawn) {
                        if (_levelManager.GetShopSpawn()) {
                            exit.SetStageType(StageType.Shop);
                            didSpecialRoomSpawn = true;
                            skipRegularRoomGen = true;
                            Debug.Log("Generated a SHOP exit");
                        }
                        else if (_levelManager.GetRestSpawn()) {
                            exit.SetStageType(StageType.Rest);
                            didSpecialRoomSpawn = true;
                            skipRegularRoomGen = true;
                            Debug.Log("Generated a REST exit");
                        }
                        else if (_levelManager.GetSirrachaSpawn()) {
                            exit.SetStageType(StageType.Sirracha);
                            didSpecialRoomSpawn = true;
                            skipRegularRoomGen = true;
                            Debug.Log("Generated a SIR RACHA exit");
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
                            Debug.Log("Generated a MEDIUM exit");
                        }
                        else if (randomStageVal > HardStageSpawnRate && !didHardStageSpawn) {
                            exit.SetStageType(StageType.Hard);
                            didHardStageSpawn = true;
                            Debug.Log("Generated a HARD exit");
                        }
                        else if (randomStageVal > EasyStageSpawnRate && !didEasyStageSpawn) {
                            exit.SetStageType(StageType.Easy);
                            didEasyStageSpawn = true;
                            Debug.Log("Generated an EASY exit");
                        }
                        else if (randomStageVal > SurvivalStageSpawnRate && !didSurvivalStageSpawn) {
                            exit.SetStageType(StageType.Survival);
                            didSurvivalStageSpawn = true;
                            Debug.Log("Generated a SURVIVAL exit");
                        } else {
                            // GG stage generation failed lets go again
                            goto attemptGeneration;  
                        }
                    }
                }
            }
        }

        private IEnumerator StartSurvivalTimer() {
            yield return new WaitForSeconds(SurvivalTimer);
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